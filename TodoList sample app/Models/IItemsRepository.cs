using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IItemsRepository {
        Task<IEnumerable<TodoItem>> GetOrderedItemsForDay(TodoDay day);
        Task<TodoItem> Refresh(TodoItem item);
        Task Add(TodoItem item);
        Task Update(TodoItem item);
        Task Remove(TodoItem item);
    }
}
