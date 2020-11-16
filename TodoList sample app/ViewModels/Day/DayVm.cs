using Autofac;
using Prism.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class DayVm : AAsyncLoadVm, IDayVm {
        IItemsRepository itemsRepo;
        TodoDay day;
        ILifetimeScope scope;

        IEnumerable<TodoItem> items;

        public DayVm(IItemsRepository itemsRepo, TodoDay day, ILifetimeScope scope) {
            this.itemsRepo = itemsRepo;
            this.day = day;
            this.scope = scope;

            NewTaskCmd = new DelegateCommand(NewTask, () => true);
        }

        public TodoDay Day => day;

        public IEnumerable<TodoItem> Items {
            get {
                return items;
            }
            private set {
                items = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewTaskCmd { get; }

        protected async override Task LoadAction() {
            Items = await itemsRepo.GetOrderedItemsForDay(day);
        }

        async void NewTask() {
            TodoItem item = new TodoItem(day);
            await itemsRepo.Add(item);
            scope.Resolve<IMainVm>().GoToItem(item);
        }
    }
}
