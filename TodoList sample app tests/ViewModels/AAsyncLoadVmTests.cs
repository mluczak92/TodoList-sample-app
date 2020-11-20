using System.Threading.Tasks;
using TodoList_sample_app.ViewModels;
using Xunit;

namespace TodoList_sample_app_tests.ViewModels {
    public class AAsyncLoadVmTests {
        class ConcreteAsyncLoadVm : AAsyncLoadVm {
            protected override Task LoadAction() {
                LoadActionExecuted = true;
                return Task.FromResult<object>(null);
            }

            public bool LoadActionExecuted { get; private set; }
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
            Assert.True(vm.LoadActionExecuted);
        }
    }
}
