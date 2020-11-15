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
    class CalendarVm : ICalendarVm, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        IDaysRepository daysRepo;
        IBoundriesSelector boundriesSelector;

        DateTime selectedMonth;
        DateTime minMonth;
        DateTime maxMonth;

        IEnumerable<TodoDayPresenter> days;

        public CalendarVm(IDaysRepository daysRepo, IItemsRepository itemsRepo,
            IBoundriesSelector boundriesSelector, TodoDay day = null) {
            this.daysRepo = daysRepo;
            this.boundriesSelector = boundriesSelector;

            if (day == null)
                selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else
                selectedMonth = new DateTime(day.Day.Year, day.Day.Month, 1);

            minMonth = new DateTime(2000, 1, 1);
            maxMonth = new DateTime(2049, 12, 1);

            LoadedCbCmd = new DelegateCommand(RefreshDays, () => true);
            NextMonthCmd = new DelegateCommand(
                () => SelectedMonth = selectedMonth.AddMonths(1),
                () => selectedMonth < maxMonth);
            PrevMonthCmd = new DelegateCommand(
                () => SelectedMonth = selectedMonth.AddMonths(-1),
                () => selectedMonth > minMonth);
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

        public IEnumerable<TodoDayPresenter> Days {
            get {
                return days;
            }
            private set {
                days = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedCbCmd { get; }
        public ICommand NextMonthCmd { get; }
        public ICommand PrevMonthCmd { get; }

        void RefreshDays() {
            boundriesSelector.Select(selectedMonth.Year, selectedMonth.Month,
                out DateTime min, out DateTime max);

            Days = daysRepo.Days.Include(x => x.Items)
                .Where(x => x.Day.Date >= min &&
                    x.Day.Date <= max)
                .OrderBy(x => x.Day)
                .ToList()
                .Select(x => new TodoDayPresenter(selectedMonth.Month, x));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
