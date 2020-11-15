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
                  $"You can provide another connection string as application argument.", innerEx) { }
    }

    class DatabaseMigrator : IDatabaseMigrator {
        TodoContext context;

        public DatabaseMigrator(TodoContext context) {
            this.context = context;
        }

        async public Task EnsureMigrated() {
            await CheckSchema();
            await CheckInitData();
        }

        async Task CheckSchema() {
            try {
                await context.Database.MigrateAsync();
            } catch (SqlException ex) when (ex.Number == -1 || ex.Number == 2 || ex.Number == 53) {
                // 20.11.14: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/cc645611(v=sql.105)
                throw new ServerConnectionException(ex, context.Database.GetDbConnection().ConnectionString);
            } catch (SqlException ex) {
                throw new MigrationException(ex, context.Database.GetDbConnection().ConnectionString);
            }
        }

        async Task CheckInitData() {
            if (!await context.Days.AnyAsync()) {
                await Fetch();
            }
        }

        async Task Fetch() {
            DateTime max = new DateTime(2049, 12, 31);
            for (DateTime i = new DateTime(2000, 1, 1); i <= max; i = i.AddDays(1)) {
                TodoDay newDay = new TodoDay() {
                    Day = i,
                };

                if (i.Date == DateTime.Now.Date) {
                    newDay.Items.Add(new TodoItem() {
                        Time = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes),
                        Note = "This is your first task for today!"
                    });
                }

                context.Days.Add(newDay);
            }
            await context.SaveChangesAsync();
        }
    }
}
