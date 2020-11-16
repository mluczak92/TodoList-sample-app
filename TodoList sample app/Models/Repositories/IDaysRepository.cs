using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IDaysRepository {
        Task<IEnumerable<TodoDay>> GetOrderedDaysWithItems(Expression<Func<TodoDay, bool>> condition);
    }
}
