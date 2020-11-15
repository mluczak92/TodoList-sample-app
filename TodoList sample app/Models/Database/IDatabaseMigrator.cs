using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    interface IDatabaseMigrator {
        Task EnsureMigrated();
    }
}
