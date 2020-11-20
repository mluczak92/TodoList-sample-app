using Autofac;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    public class DayVm : AAsyncLoadVm, IDayVm {
        IItemsRepository itemsRepo;
        TodoDay day;
        ILifetimeScope scope;

        IEnumerable<TodoItem> items;

        public DayVm(IItemsRepository itemsRepo, TodoDay day, ILifetimeScope scope) {
            this.itemsRepo = itemsRepo;
            this.day = day;
            this.scope = scope;

            NewTaskCmd = new DelegateCommand(NewTask, CanAddNew);
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

        public async override Task LoadAction() {
            Items = await itemsRepo.GetOrderedItems(x => x.Day == day);
        }

        async void NewTask() {
            if (!CanAddNew()) {
                return;
            }

            DateTime selectedDate = day.Day;
            TimeSpan now = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes);

            TodoItem item = new TodoItem() {
                DayId = day.Id,
                Time = now.Add(TimeSpan.FromMinutes(60)),
                ReminderTime = selectedDate.Add(now.Add(TimeSpan.FromMinutes(30))),
                Note = "new task"
            };

            await itemsRepo.Add(item);
            scope.Resolve<IMainVm>().GoToItem(item);
        }

        bool CanAddNew() {
            return day.Day.Date >= DateTime.Now.Date;
        }
    }
}
