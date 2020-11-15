using System;

namespace TodoList_sample_app.Models.Database {
    class TodoItem {
        public int Id { get; set; }
        public string Note { get; set; }
        public TimeSpan Time { get; set; }
        public TodoDay Day { get; set; }
    }
}
