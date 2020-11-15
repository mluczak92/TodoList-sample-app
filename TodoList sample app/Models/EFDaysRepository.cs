using System.Linq;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    class EFDaysRepository : IDaysRepository {
        TodoContext context;

        public EFDaysRepository(TodoContext context) {
            this.context = context;
        }

        public IQueryable<TodoDay> Days => context.Days;
    }
}
