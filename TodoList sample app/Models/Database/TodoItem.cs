using System;

namespace TodoList_sample_app.Models.Database {
    class TodoItem : IEquatable<TodoItem> {
        public int Id { get; set; }
        public string Note { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime? ReminderTime { get; set; }
        public int DayId { get; set; }
        public TodoDay Day { get; set; }

        public bool Equals(TodoItem other) {
            if (other == null)
                return false;

            return Time == other.Time && Note == other.Note;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            TodoItem itemObj = obj as TodoItem;
            if (itemObj == null)
                return false;
            else
                return Equals(itemObj);
        }

        public override int GetHashCode() {
            int timeHash = Time.GetHashCode();
            int noteHash = Note?.GetHashCode() ?? 0;
            return (timeHash ^ noteHash).GetHashCode();
        }

        public TodoItem ShallowCopy() {
            return (TodoItem)MemberwiseClone();
        }
    }
}
