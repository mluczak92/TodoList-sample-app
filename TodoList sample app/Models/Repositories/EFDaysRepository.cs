using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    public class EFDaysRepository : IDaysRepository {
        ILifetimeScope scope;

        public EFDaysRepository(ILifetimeScope scope) {
            this.scope = scope;
        }

        public async Task<IEnumerable<TodoDay>> GetOrderedDaysWithItems(Expression<Func<TodoDay, bool>> condition) {
            using ILifetimeScope nested = scope.BeginLifetimeScope();
            using TodoContext context = nested.Resolve<TodoContext>();
            return await context.Days
                .Include(x => x.Items)
                .Where(condition)
                .OrderBy(x => x.Day)
                .ToListAsync();
        }
    }
}
