using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IItemsRepository {
        Task Add(TodoItem item);
    }
}
