using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoList_sample_app.ViewModels {
    abstract class AAsyncLoadVm : ANotifyPropChanged {
        bool initStarted;

        public AAsyncLoadVm() {
            LoadedCbCmd = new DelegateCommand(Load, () => !initStarted);
        }

        async void Load() {
            if (!initStarted) {
                initStarted = true;
                await LoadAction();
            }
        }

        protected abstract Task LoadAction();

        public ICommand LoadedCbCmd { get; }
    }
}
