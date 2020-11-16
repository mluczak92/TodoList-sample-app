using Microsoft.EntityFrameworkCore;

namespace TodoList_sample_app.Models.Database {
    class TodoContext : DbContext {
        string conString;

#if DEBUG //constructor used to create migrations in dev env;
        public TodoContext() {
            conString = "Server=(local); Database=TodoList; Integrated Security=true";
        }
#endif

        public TodoContext(string conString) : base() {
            this.conString = conString;
        }

        public DbSet<TodoDay> Days { get; set; }
        public DbSet<TodoItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(conString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TodoDay>()
                .Property(x => x.Day)
                .HasColumnType("date");

            modelBuilder.Entity<TodoDay>()
                .HasIndex(x => x.Day)
                .IsUnique(true)
                .IsClustered(false);

            modelBuilder.Entity<TodoNotification>()
                .HasIndex(x => x.IsActive)
                .IsUnique(false)
                .IsClustered(false);
        }
    }
}
