using Autofac;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    public abstract class DatabaseException : Exception {
        public DatabaseException(string msg, Exception innerEx) : base(msg, innerEx) { }
    }

    public class ServerConnectionException : DatabaseException {
        public ServerConnectionException(Exception innerEx, string conString) :
            base($"Cannot connect to database server using connection string: \"{conString}\".\n\n" +
                $"You can provide another connection string by passing it as application argument.", innerEx) { }
    }

    public class MigrationException : DatabaseException {
        public MigrationException(Exception innerEx, string conString)
            : base($"Cannot apply migrations using connection string: \"{conString}\".\n\n" +
                  $"You can provide another connection string by passing it as application argument.", innerEx) { }
    }

    public class DatabaseMigrator : IDatabaseMigrator {
        ILifetimeScope scope;
        IMigrateAsyncWrapper migrateAsyncWrapper;

        public DatabaseMigrator(ILifetimeScope scope, IMigrateAsyncWrapper migrateAsyncWrapper) {
            this.scope = scope;
            this.migrateAsyncWrapper = migrateAsyncWrapper;
        }

        async public Task EnsureMigrated() {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            await CheckSchema(context);
            await CheckInitData(context);
        }

        async Task CheckSchema(TodoContext context) {
            try {
                await migrateAsyncWrapper.MigrateAsync(context.Database);
            } catch (SqlException ex) when (ex.Number == -1 || ex.Number == 2 || ex.Number == 53) {
                // 20.11.14: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/cc645611(v=sql.105)
                throw new ServerConnectionException(ex, context.Database?.GetDbConnection()?.ConnectionString);
            } catch (SqlException ex) {
                throw new MigrationException(ex, context.Database?.GetDbConnection()?.ConnectionString);
            }
        }

        async Task CheckInitData(TodoContext context) {
            if (!await context.Days.AnyAsync()) {
                await Fetch(context);
            }
        }

        async Task Fetch(TodoContext context) {
            DateTime max = new DateTime(2030, 1, 31);
            for (DateTime i = new DateTime(2019, 12, 1); i <= max; i = i.AddDays(1)) {
                TodoDay newDay = new TodoDay() {
                    Day = i,
                };

                CreateInitTasks(newDay);
                context.Days.Add(newDay);
            }
            await context.SaveChangesAsync();
        }

        void CreateInitTasks(TodoDay day) {
            if (day.Day.Date == DateTime.Now.Date) {
                DateTime today = day.Day.Date;
                TimeSpan now = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes);

                day.Items.Add(new TodoItem() {
                    Time = now.Add(TimeSpan.FromMinutes(60)),
                    ReminderTime = today.Add(now.Add(TimeSpan.FromMinutes(30))),
                    Note = "This is your first task note for today!\n\n" +
                    "" +
                    "You can click on it and change its content or planned time."
                });

                day.Items.Add(new TodoItem() {
                    Time = now.Add(TimeSpan.FromMinutes(120)),
                    ReminderTime = today.Add(now.Add(TimeSpan.FromMinutes(90))),
                    Note = "You can also create a new note or move back to the calendar page, using buttons in the right top corner of this view."
                });
            }
        }
    }
}
