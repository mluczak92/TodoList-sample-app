using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoList_sample_app.ViewModels {
    public interface IAsyncLoadVm {
        public bool LoadingStarted { get; }
        public ICommand LoadedCbCmd { get; }
    }

    public abstract class AAsyncLoadVm : ANotifyPropChanged, IAsyncLoadVm {
        public AAsyncLoadVm() {
            LoadedCbCmd = new DelegateCommand(LoadAsync, () => !LoadingStarted);
        }

        public bool LoadingStarted { get; private set; }

        async void LoadAsync() {
            if (LoadingStarted) {
                return;
            }

            LoadingStarted = true;
            await LoadAction();
        }

        protected abstract Task LoadAction();

        public ICommand LoadedCbCmd { get; }
    }
}
