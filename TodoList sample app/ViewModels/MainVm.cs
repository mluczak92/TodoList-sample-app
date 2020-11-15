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
    class MainVm : IMainVm, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        IDatabaseMigrator dbChecker;
        IDaysRepository daysRepo;
        IItemsRepository itemsRepo;

        bool isLoaded;
        DateTime startDate;

        IEnumerable<TodoDay> days;
        TodoDay selectedDay;
        IEnumerable<TodoItem> items;
        TodoItem selectedItem;

        public MainVm(IDatabaseMigrator dbChecker, IDaysRepository daysRepo,
            IItemsRepository itemsRepo) {
            this.dbChecker = dbChecker;
            this.daysRepo = daysRepo;
            this.itemsRepo = itemsRepo;

            bool isLoaded = true;
            startDate = DateTime.Now.Date;

            LoadedCbCmd = new DelegateCommand(LoadedCb, () => isLoaded);
            NewTaskCmd = new DelegateCommand(NewTask, () => true);
        }

        public DateTime StartDate {
            get {
                return startDate;
            }
            set {
                startDate = value;
                OnPropertyChanged();
                RefreshDays();
            }
        }

        public bool IsLoaded {
            get {
                return isLoaded;
            }
            private set {
                isLoaded = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TodoDay> Days {
            get {
                return days;
            }
            private set {
                days = value;
                OnPropertyChanged();
            }
        }

        public TodoDay SelectedDay {
            get {
                return selectedDay;
            }
            set {
                selectedDay = value;
                OnPropertyChanged();
                RefreshItems();
            }
        }

        public IEnumerable<TodoItem> Items {
            get {
                return items;
            }
            private set {
                items = value?.OrderBy(x => x.Time)
                    .ThenBy(x => x.Id); //create time
                OnPropertyChanged();
            }
        }

        public TodoItem SelectedItem {
            get {
                return selectedItem;
            }
            set {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedCbCmd { get; private set; }
        public ICommand NewTaskCmd { get; private set; }

        async void LoadedCb() {
            try {
                IsLoaded = false;
                await dbChecker.EnsureMigrated();
                RefreshDays();
            } finally {
                IsLoaded = true;
            }
        }

        void RefreshDays() {
            Days = daysRepo.Days
                .Include(x => x.Items)
                .Where(x => x.Day.Date >= startDate)
                .Take(14)
                .ToList();
            SelectedDay = days.FirstOrDefault();
        }

        async void NewTask() {
            TodoItem newItem = new TodoItem() {
                Time = DateTime.Now.TimeOfDay,
                Name = "New task",
            };
            await itemsRepo.Add(selectedDay, newItem);
            RefreshItems(newItem);
        }

        void RefreshItems(TodoItem item = null) {
            Items = selectedDay?.Items;
            if (item == null)
                SelectedItem = items?.FirstOrDefault();
            else
                SelectedItem = item;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
