using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class CalendarVm : AAsyncLoadVm, ICalendarVm {
        IDaysRepository daysRepo;
        IBoundriesSelector boundriesSelector;

        DateTime selectedMonth;
        DateTime minMonth;
        DateTime maxMonth;

        IEnumerable<TodoDayPresenter> days;

        public CalendarVm(IDaysRepository daysRepo, IBoundriesSelector boundriesSelector, TodoDay day = null) {
            this.daysRepo = daysRepo;
            this.boundriesSelector = boundriesSelector;

            if (day == null)
                selectedMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else
                selectedMonth = new DateTime(day.Day.Year, day.Day.Month, 1);

            minMonth = new DateTime(2020, 1, 1);
            maxMonth = new DateTime(2029, 12, 1);

            NextMonthCmd = new DelegateCommand(AddMonth, CanAddMonth);
            PrevMonthCmd = new DelegateCommand(SubMonth, CanSubMonth);
        }

        public DateTime SelectedMonth {
            get {
                return selectedMonth;
            }
            private set {
                selectedMonth = value;
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

        public DelegateCommand NextMonthCmd { get; }
        public DelegateCommand PrevMonthCmd { get; }

        protected async override Task LoadAction() {
            boundriesSelector.Select(selectedMonth.Year, selectedMonth.Month,
                out DateTime min, out DateTime max);

            Days = (await daysRepo.GetOrderedDaysWithItems(x => x.Day.Date >= min && x.Day.Date <= max))
                .Select(x => new TodoDayPresenter(selectedMonth.Month, x));
        }

        async void AddMonth() {
            if (!CanAddMonth()) {
                return;
            }

            SelectedMonth = selectedMonth.AddMonths(1);
            await RefreshDaysAndInvalidateCmds();
        }

        bool CanAddMonth() {
            return selectedMonth < maxMonth;
        }

        async void SubMonth() {
            if (!CanSubMonth()) {
                return;
            }

            SelectedMonth = selectedMonth.AddMonths(-1);
            await RefreshDaysAndInvalidateCmds();
        }

        async Task RefreshDaysAndInvalidateCmds() {
            await LoadAction();
            NextMonthCmd.RaiseCanExecuteChanged();
            PrevMonthCmd.RaiseCanExecuteChanged();
        }

        bool CanSubMonth() {
            return selectedMonth > minMonth;
        }
    }
}
