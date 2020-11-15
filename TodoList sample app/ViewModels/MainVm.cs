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
        IBoundriesSelector boundriesSelector;

        bool isLoaded;
        DateTime selectedMonth;
        DateTime minMonth;
        DateTime maxMonth;

        IEnumerable<TodoDayPresenter> days;
        TodoDayPresenter selectedDay;
        IEnumerable<TodoItem> items;
        TodoItem selectedItem;

        public MainVm(IDatabaseMigrator dbChecker, IDaysRepository daysRepo,
            IItemsRepository itemsRepo, IBoundriesSelector boundriesSelector) {
            this.dbChecker = dbChecker;
            this.daysRepo = daysRepo;
            this.itemsRepo = itemsRepo;
            this.boundriesSelector = boundriesSelector;

            bool isLoaded = true;
            selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            minMonth = new DateTime(2000, 1, 1);
            maxMonth = new DateTime(2049, 12, 1);

            LoadedCbCmd = new DelegateCommand(LoadedCb, () => isLoaded);
            NextMonthCmd = new DelegateCommand(
                () => SelectedMonth = selectedMonth.AddMonths(1),
                () => selectedMonth < maxMonth);
            PrevMonthCmd = new DelegateCommand(
                () => SelectedMonth = selectedMonth.AddMonths(-1),
                () => selectedMonth > minMonth);
            NewTaskCmd = new DelegateCommand(NewTask, () => true);
        }

        public DateTime SelectedMonth {
            get {
                return selectedMonth;
            }
            private set {
                selectedMonth = value;
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

        public IEnumerable<TodoDayPresenter> Days {
            get {
                return days;
            }
            private set {
                days = value;
                OnPropertyChanged();
            }
        }

        public TodoDayPresenter SelectedDay {
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

        public ICommand LoadedCbCmd { get; }
        public ICommand NextMonthCmd { get; }
        public ICommand PrevMonthCmd { get; }
        public ICommand NewTaskCmd { get; }

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
            boundriesSelector.Select(selectedMonth.Year, selectedMonth.Month,
                out DateTime min, out DateTime max);
            Days = daysRepo.Days.Include(x => x.Items)
                .Where(x => x.Day.Date >= min &&
                    x.Day.Date <= max)
                .OrderBy(x => x.Day)
                .ToList()
                .Select(x => new TodoDayPresenter(selectedMonth.Month, x));
            SelectedDay = days.FirstOrDefault();
        }

        async void NewTask() {
            TodoItem newItem = new TodoItem() {
                Day = selectedDay.Original,
                Time = DateTime.Now.TimeOfDay,
                Note = "New task",
            };
            await itemsRepo.Add(newItem);
            RefreshItems(newItem);
        }

        void RefreshItems(TodoItem item = null) {
            Items = selectedDay?.Original.Items;
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
