using System.Linq;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IItemsRepository {
        IQueryable<TodoItem> GetItems(TodoDay day);
        Task Add(TodoItem item);
    }
}
