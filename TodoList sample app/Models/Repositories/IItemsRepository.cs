using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface IItemsRepository {
        Task<IEnumerable<TodoItem>> GetOrderedItems(Expression<Func<TodoItem, bool>> condition);
        Task<TodoItem> Refresh(TodoItem item);
        Task Add(TodoItem item);
        Task Update(TodoItem item);
        Task Remove(TodoItem item);
    }
}
