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

        IEnumerable<TodoItem> items;

        public DayVm(IItemsRepository itemsRepo, TodoDay day) {
            this.itemsRepo = itemsRepo;
            this.day = day;

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
                .ToList();
        }

        async void NewTask() {
            TodoItem newItem = new TodoItem() {
                Day = day,
                Time = TimeSpan.FromMinutes((int)DateTime.Now.TimeOfDay.TotalMinutes),
                Note = "New task",
            };
            await itemsRepo.Add(newItem);
            RefreshItems();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
