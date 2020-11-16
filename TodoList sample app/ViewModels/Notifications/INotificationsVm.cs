using System.Collections.Generic;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    interface INotificationsVm : ITodoVm {
        void Add(IEnumerable<TodoItem> notifs);
    }
}
