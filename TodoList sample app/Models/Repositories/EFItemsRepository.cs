using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    public class EFItemsRepository : IItemsRepository {
        ILifetimeScope scope;

        public EFItemsRepository(ILifetimeScope scope) {
            this.scope = scope;
        }

        public async Task<IEnumerable<TodoItem>> GetOrderedItems(Expression<Func<TodoItem, bool>> condition) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            return await context.Items
                .Include(x => x.Day)
                .Where(condition)
                .OrderBy(x => x.Day.Day)
                .ThenBy(x => x.Time)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<TodoItem> Refresh(TodoItem item) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            await context.Entry(item).ReloadAsync();
            await context.Entry(item).Reference(x => x.Day).LoadAsync();
            return item;
        }

        public async Task Add(TodoItem item) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            context.Items.Add(item);
            await context.SaveChangesAsync();
        }

        public async Task Update(TodoItem item) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            context.Items.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task Remove(TodoItem item) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            context.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
