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
                .Where(x => x.DayId == day.Id);
        }

        public TodoItem ReloadItem(TodoItem item) {
            context.Entry(item).Reload();
            return item;
        }

        public async Task Add(TodoItem item) {
            context.Items.Add(item);
            await context.SaveChangesAsync();
            await context.Entry(item).Reference(x => x.Day).LoadAsync();
        }

        public async Task<TodoItem> Update(TodoItem item) {
            context.Items.Update(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task Remove(TodoItem item) {
            context.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
