using System;
using System.Collections.Generic;

namespace TodoList_sample_app.Models.Database {
    class TodoItem {
        public TodoItem() {
            Time = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes)
                .Add(TimeSpan.FromMinutes(15));
            Note = "new task";
        }

        public TodoItem(TodoDay day) : this() {
            DayId = day.Id;
        }

        public int Id { get; set; }
        public string Note { get; set; }
        public TimeSpan Time { get; set; }
        public int DayId { get; set; }
        public TodoDay Day { get; set; }
        public ICollection<TodoNotification> Notifications { get; set; } = new List<TodoNotification>();
    }
}
