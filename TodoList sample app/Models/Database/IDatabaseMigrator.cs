using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    public interface IDatabaseMigrator {
        Task EnsureMigrated();
    }
}
