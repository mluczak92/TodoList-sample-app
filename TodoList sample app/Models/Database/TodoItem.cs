using System;

namespace TodoList_sample_app.Models.Database {
    class TodoItem {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan Time { get; set; }
    }
}
