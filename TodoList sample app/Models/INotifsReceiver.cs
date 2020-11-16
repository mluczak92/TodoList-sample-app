using System.Collections.Generic;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    interface INotifsReceiver {
        void Receive(IEnumerable<TodoItem> notifs);
    }
}
