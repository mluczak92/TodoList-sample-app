using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    public interface IMigrateAsyncWrapper {
        Task MigrateAsync(DatabaseFacade databaseFacade);
    }
}
