using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    class EFItemsRepository : IItemsRepository {
        TodoContext context;

        public EFItemsRepository(TodoContext context) {
            this.context = context;
        }

        public async Task Add(TodoItem item) {
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }
    }
}
