using System.Linq;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IDaysRepository {
        IQueryable<TodoDay> Days { get; }
    }
}
