namespace TodoList_sample_app.Models.Database {
    class TodoNotification {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int ItemId { get; set; }
        public TodoItem Item { get; set; }
    }
}
