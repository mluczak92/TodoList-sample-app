using System;

namespace TodoList_sample_app.Models.Database {
    class TodoDayPresenter {
        TodoDay wrapped;
        int selectedMonth;

        public TodoDayPresenter(int selectedMonth, TodoDay day) {
            wrapped = day;
            this.selectedMonth = selectedMonth;
        }

        public TodoDay Original => wrapped;
        public bool IsDayFromSelectedMonth => wrapped.Day.Month == selectedMonth;
        public bool IsWeekend => (int)wrapped.Day.DayOfWeek == 6 || wrapped.Day.DayOfWeek == 0;
        public bool IsToday => wrapped.Day.Date == DateTime.Now.Date;
    }
}
