using System;
using System.Collections.Generic;

namespace TodoList_sample_app.Models.Database {
    public class TodoDay {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}
