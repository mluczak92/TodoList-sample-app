using System;
using TodoList_sample_app.Models;
using Xunit;

namespace TodoList_sample_app_tests.Models {
    public class DatesBoundriesSelectorTests {

        [Theory]
        [InlineData(2020, 1, 2, 2)]
        [InlineData(2020, 2, 5, 1)]
        [InlineData(2020, 3, 6, 5)]
        [InlineData(2020, 4, 2, 3)]
        [InlineData(2020, 5, 4, 7)]
        [InlineData(2020, 6, 7, 5)]
        [InlineData(2020, 7, 2, 2)]
        [InlineData(2020, 8, 5, 6)]
        [InlineData(2020, 9, 1, 4)]
        [InlineData(2020, 10, 3, 1)]
        [InlineData(2020, 11, 6, 6)]
        [InlineData(2020, 12, 1, 3)]
        [InlineData(2021, 2, 7, 7)]
        public void ShouldAddDaysBeforeAndAfterMonth(int year, int month,
            int daysBefore, int daysAfter) {
            DatesBoundriesSelector selector = new DatesBoundriesSelector();

            selector.Select(year, month, out DateTime min, out DateTime max);

            DateTime firstDay = new DateTime(year, month, 1).AddDays(-daysBefore);
            DateTime lastDay = new DateTime(year, month, 1).AddMonths(1).AddDays(daysAfter - 1);
            Assert.Equal(firstDay, min);
            Assert.Equal(lastDay, max);
        }
    }
}
