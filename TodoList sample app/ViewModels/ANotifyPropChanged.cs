using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoList_sample_app.ViewModels {
    abstract class ANotifyPropChanged : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
