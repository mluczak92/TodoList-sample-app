using System;

namespace TodoList_sample_app.Models {
    class CalendarLikeBoundriesSelector : IBoundriesSelector {
        public void Select(int year, int month, out DateTime min, out DateTime max) {
            DateTime firstDay = new DateTime(year, month, 1);

            int daysFromPrevMonth = DayOfWeekIndex(firstDay);
            if (daysFromPrevMonth == 0)
                daysFromPrevMonth = 7;
            min = firstDay.AddDays(-1 * daysFromPrevMonth);

            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            int daysFromNextMonth = 6 - DayOfWeekIndex(lastDay);
            if (daysFromNextMonth == 0)
                daysFromNextMonth = 7;
            max = lastDay.AddDays(daysFromNextMonth);
        }

        int DayOfWeekIndex(DateTime date) {
            int tmp = (int)date.DayOfWeek;
            if (tmp == 0)
                tmp = 7;
            return --tmp;
        }
    }
}
