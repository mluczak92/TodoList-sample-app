using Autofac;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class DayVm : IDayVm, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        IItemsRepository itemsRepo;
        TodoDay day;
        ILifetimeScope scope;

        IEnumerable<TodoItem> items;

        public DayVm(IItemsRepository itemsRepo, TodoDay day, ILifetimeScope scope) {
            this.itemsRepo = itemsRepo;
            this.day = day;
            this.scope = scope;

            LoadedCbCmd = new DelegateCommand(RefreshItems, () => true);
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

        public ICommand LoadedCbCmd { get; }
        public ICommand NewTaskCmd { get; }

        void RefreshItems() {
            Items = itemsRepo.GetItems(day)
                .Include(x => x.Day) 
                .OrderBy(x => x.Time)
                .ThenBy(x => x.Id)
                .ToList();
        }

        async void NewTask() {
            TodoItem newItem = new TodoItem() {
                DayId = day.Id,
                Time = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes),
                Note = "new task",
            };
            await itemsRepo.Add(newItem);
            scope.Resolve<IMainVm>().GoToItem(newItem);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
