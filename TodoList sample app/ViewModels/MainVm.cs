using Autofac;
using Prism.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class MainVm : IMainVm, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        IDatabaseMigrator dbChecker;
        ILifetimeScope scope;

        ITodoVm currentVm;

        public MainVm(IDatabaseMigrator dbChecker, ILifetimeScope scope) {
            this.dbChecker = dbChecker;
            this.scope = scope;

            LoadedCbCmd = new DelegateCommand(LoadedCb, () => true);
            GotoDayCmd = new DelegateCommand<TodoDay>(GoToDay, x => true);
            GotoCalendarCmd = new DelegateCommand<TodoDay>(GoToCalendar, x => true);
        }

        public ITodoVm CurrentVm {
            get {
                return currentVm;
            }
            set {
                currentVm = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedCbCmd { get; }
        public ICommand GotoDayCmd { get; }
        public ICommand GotoCalendarCmd { get; }

        async void LoadedCb() {
            await dbChecker.EnsureMigrated();
            CurrentVm = scope.Resolve<ICalendarVm>();
        }

        void GoToDay(TodoDay day) {
            CurrentVm = scope.Resolve<IDayVm>(new TypedParameter(typeof(TodoDay), day));
        }

        void GoToCalendar(TodoDay day) {
            CurrentVm = scope.Resolve<ICalendarVm>(new TypedParameter(typeof(TodoDay), day));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
