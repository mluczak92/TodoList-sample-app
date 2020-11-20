using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoList_sample_app.ViewModels {
    public abstract class AAsyncLoadVm : ANotifyPropChanged {
        bool initStarted;

        public AAsyncLoadVm() {
            LoadedCbCmd = new DelegateCommand(Load, () => !initStarted);
        }

        async void Load() {
            if (initStarted) {
                return;
            }

            initStarted = true;
            await LoadAction();
        }

        public abstract Task LoadAction();

        public ICommand LoadedCbCmd { get; }
    }
}
