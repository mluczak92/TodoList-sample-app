using System.Threading.Tasks;
using TodoList_sample_app.ViewModels;
using Xunit;

namespace TodoList_sample_app_tests.ViewModels {
    public class AAsyncLoadVmTests {
        class ConcreteAsyncLoadVm : AAsyncLoadVm {
            protected override Task LoadAction() {
                ExecutedTimes++;
                return Task.FromResult<object>(null);
            }

            public int ExecutedTimes { get; private set; }
        }

        [Fact]
        public void LoadingStartedInitFalse() {
            AAsyncLoadVm vm = new ConcreteAsyncLoadVm();

            Assert.False(vm.LoadingStarted);
        }

        [Fact]
        public void LoadingStartedChangeCmdExecute() {
            ConcreteAsyncLoadVm vm = new ConcreteAsyncLoadVm();

            vm.LoadedCbCmd.Execute(null);

            Assert.True(vm.LoadingStarted);
            Assert.Equal(1, vm.ExecutedTimes);
        }

        [Fact]
        public void SingleLoad() {
            ConcreteAsyncLoadVm vm = new ConcreteAsyncLoadVm();

            vm.LoadedCbCmd.Execute(null);
            vm.LoadedCbCmd.Execute(null);

            Assert.Equal(1, vm.ExecutedTimes);
        }
    }
}
