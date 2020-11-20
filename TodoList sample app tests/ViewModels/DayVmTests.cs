using Autofac;
using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;
using TodoList_sample_app.ViewModels;
using Xunit;

namespace TodoList_sample_app_tests.ViewModels {
    public class DayVmTests {

        [Theory]
        [InlineData(-999, false)]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(999, true)]
        public void NewTaskCmdCanExecute(int daysFromToday, bool expected) {
            using AutoMock mock = AutoMock.GetLoose();
            TodoDay day = new TodoDay() {
                Day = DateTime.Now.AddDays(daysFromToday)
            };
            DayVm vm = mock.Create<DayVm>(TypedParameter.From(day));
            
            bool result = vm.NewTaskCmd.CanExecute(null);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NewTaskCmdDontExecute() {
            using AutoMock mock = AutoMock.GetStrict(x => {
                x.RegisterType<MainVm>()
                    .As<IMainVm>()
                    .SingleInstance();
            });
            mock.Mock<IItemsRepository>()
                .Setup(x => x.Add(It.IsAny<TodoItem>()))
                .Returns(Task.FromResult<object>(null));
            TodoDay day = new TodoDay() {
                Day = DateTime.Now.AddDays(-1)
            };
            DayVm vm = mock.Create<DayVm>(TypedParameter.From(day));

            vm.NewTaskCmd.Execute(null);

            IMainVm main = mock.Create<IMainVm>();
            Assert.Null(main.CurrentVm);
        }

        [Fact]
        public void NewTaskCmdExecute() {
            using AutoMock mock = AutoMock.GetStrict(x => {
                x.RegisterType<MainVm>()
                    .As<IMainVm>()
                    .SingleInstance();
            });
            mock.Mock<IItemsRepository>()
                .Setup(x => x.Add(It.IsAny<TodoItem>()))
                .Returns(Task.FromResult<object>(null));
            TodoDay day = new TodoDay() {
                Day = DateTime.Now
            };
            DayVm vm = mock.Create<DayVm>(TypedParameter.From(day));

            vm.NewTaskCmd.Execute(null);

            IMainVm main = mock.Create<IMainVm>();
            Assert.IsAssignableFrom<IItemVm>(main.CurrentVm);
        }

        [Fact]
        public async Task LoadAction() {
            using AutoMock mock = AutoMock.GetLoose();
            IEnumerable<TodoItem> items = Enumerable.Repeat(new TodoItem(), 5);
            mock.Mock<IItemsRepository>()
                .Setup(x => x.GetOrderedItems(It.IsAny<Expression<Func<TodoItem, bool>>>()))
                .Returns(Task.FromResult(items));
            DayVm vm = mock.Create<DayVm>();

            await vm.LoadAction();

            Assert.Same(items, vm.Items);
        }

        [Fact]
        public void DayProperty() {
            using AutoMock mock = AutoMock.GetLoose();
            TodoDay day = new TodoDay() {
                Day = DateTime.Now
            };

            DayVm vm = mock.Create<DayVm>(TypedParameter.From(day));

            Assert.Same(day, vm.Day);
        }
    }
}
