using Autofac;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    abstract class DatabaseException : Exception {
        public DatabaseException(string msg, Exception innerEx) : base(msg, innerEx) { }
    }

    class ServerConnectionException : DatabaseException {
        public ServerConnectionException(Exception innerEx, string conString) :
            base($"Cannot connect to database server using connection string: \"{conString}\".\n\n" +
                $"You can provide another connection string by passing it as application argument.", innerEx) { }
    }

    class MigrationException : DatabaseException {
        public MigrationException(Exception innerEx, string conString)
            : base($"Cannot apply migrations using connection string: \"{conString}\".\n\n" +
                  $"You can provide another connection string by passing it as application argument.", innerEx) { }
    }

    class DatabaseMigrator : IDatabaseMigrator {
        ILifetimeScope scope;

        public DatabaseMigrator(ILifetimeScope scope) {
            this.scope = scope;
        }

        async public Task EnsureMigrated() {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            await CheckSchema(context);
            await CheckInitData(context);
        }

        async Task CheckSchema(TodoContext context) {
            try {
                await context.Database.MigrateAsync();
            } catch (SqlException ex) when (ex.Number == -1 || ex.Number == 2 || ex.Number == 53) {
                // 20.11.14: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/cc645611(v=sql.105)
                throw new ServerConnectionException(ex, context.Database.GetDbConnection().ConnectionString);
            } catch (SqlException ex) {
                throw new MigrationException(ex, context.Database.GetDbConnection().ConnectionString);
            }
        }

        async Task CheckInitData(TodoContext context) {
            if (!await context.Days.AnyAsync()) {
                await Fetch(context);
            }
        }

        async Task Fetch(TodoContext context) {
            DateTime max = new DateTime(2050, 1, 31);
            for (DateTime i = new DateTime(1999, 12, 31); i <= max; i = i.AddDays(1)) {
                TodoDay newDay = new TodoDay() {
                    Day = i,
                };

                if (i.Date == DateTime.Now.Date) {
                    TimeSpan now = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes);

                    newDay.Items.Add(new TodoItem() {
                        Time = now.Add(TimeSpan.FromMinutes(60)),
                        ReminderTime = i.Add(now.Add(TimeSpan.FromMinutes(30))),
                        Note = "This is your first task for today!"
                    });
                }

                context.Days.Add(newDay);
            }
            await context.SaveChangesAsync();
        }
    }
}
