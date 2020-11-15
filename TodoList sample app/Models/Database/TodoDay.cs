using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList_sample_app.Models.Database {
    class TodoDay {
        [Key]
        [Column(TypeName = "date")]
        public DateTime Day { get; set; }
        public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}
