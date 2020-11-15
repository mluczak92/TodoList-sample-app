using System.Linq;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IItemsRepository {
        IQueryable<TodoItem> GetItems(TodoDay day);
        TodoItem ReloadItem(TodoItem item);
        Task Add(TodoItem item);
        Task<TodoItem> Update(TodoItem item);
        Task Remove(TodoItem item);
    }
}
