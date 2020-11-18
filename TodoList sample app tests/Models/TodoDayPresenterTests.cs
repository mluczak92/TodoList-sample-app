using System;
using System.Linq;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;
using Xunit;

namespace TodoList_sample_app_tests.Models {
    public class TodoDayPresenterTests {

        [Fact]
        public void SameObject() {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(2020, 1, 1)
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.Same(todoDay, presenter.Original);
        }

        [Theory]
        [InlineData(1, 2020, 1, 1, true)]
        [InlineData(2, 2020, 1, 1, false)]
        [InlineData(3, 2020, 1, 1, false)]
        [InlineData(4, 2020, 1, 1, false)]
        [InlineData(5, 2020, 1, 1, false)]
        [InlineData(6, 2020, 1, 1, false)]
        [InlineData(7, 2020, 1, 1, false)]
        [InlineData(8, 2020, 1, 1, false)]
        [InlineData(9, 2020, 1, 1, false)]
        [InlineData(10, 2020, 1, 1, false)]
        [InlineData(11, 2020, 1, 1, false)]
        [InlineData(12, 2020, 1, 1, false)]
        public void IsDayFromSelectedMonth(int selectedMonth, int year, int month, int day,
            bool expected) {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(year, month, day)
            };

            TodoDayPresenter presenter = new TodoDayPresenter(selectedMonth, todoDay);

            Assert.Equal(expected, presenter.IsDayFromSelectedMonth);
        }

        [Theory]
        [InlineData(2020, 1, 1, false)]
        [InlineData(2020, 1, 2, false)]
        [InlineData(2020, 1, 3, false)]
        [InlineData(2020, 1, 4, true)]
        [InlineData(2020, 1, 5, true)]
        [InlineData(2020, 1, 6, false)]
        [InlineData(2020, 1, 7, false)]
        public void IsWeekend(int year, int month, int day, bool expected) {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(year, month, day)
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.Equal(expected, presenter.IsWeekend);
        }

        [Fact]
        public void IsTodayTrue() {
            TodoDay todoDay = new TodoDay() {
                Day = DateTime.Now.Date
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.True(presenter.IsToday);
        }

        [Theory]
        [InlineData(2020, 1, 1)]
        [InlineData(2040, 1, 1)]
        public void IsTodayFalse(int year, int month, int day) {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(year, month, day)
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.False(presenter.IsToday);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(4, true)]
        public void AnyItems(int count, bool expected) {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(2020, 1, 1),
                Items = Enumerable.Repeat(new TodoItem(), count).ToList()
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.Equal(expected, presenter.AnyItems);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        public void ItemsCount(int count) {
            TodoDay todoDay = new TodoDay() {
                Day = new DateTime(2020, 1, 1),
                Items = Enumerable.Repeat(new TodoItem(), count).ToList()
            };

            TodoDayPresenter presenter = new TodoDayPresenter(1, todoDay);

            Assert.Equal(count, presenter.ItemsCount);
        }
    }
}
