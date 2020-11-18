using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace TodoList_sample_app.Models.Database {
    class MigrateAsyncTestable : IMigrateAsyncWrapper {
        public Task MigrateAsync(DatabaseFacade databaseFacade) {
            return databaseFacade.MigrateAsync();
        }
    }
}
