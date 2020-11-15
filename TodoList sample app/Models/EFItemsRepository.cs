using System.Linq;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    class EFItemsRepository : IItemsRepository {
        TodoContext context;

        public EFItemsRepository(TodoContext context) {
            this.context = context;
        }

        public IQueryable<TodoItem> GetItems(TodoDay day) {
            return context.Items
                .Where(x => x.Day == day);
        }

        public async Task Add(TodoItem item) {
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }
    }
}
