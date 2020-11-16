using Autofac;
using Prism.Commands;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class MainVm : AAsyncLoadVm, IMainVm, INotifyPropertyChanged {
        IDatabaseMigrator dbChecker;
        ILifetimeScope scope;

        ITodoVm currentVm;

        public MainVm(IDatabaseMigrator dbChecker, ILifetimeScope scope) {
            this.dbChecker = dbChecker;
            this.scope = scope;

            GotoDayCmd = new DelegateCommand<TodoDay>(GoToDay, x => true);
            GotoCalendarCmd = new DelegateCommand<TodoDay>(GoToCalendar, x => true);
            GotoItemCmd = new DelegateCommand<TodoItem>(GoToItem, x => true);
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

        public ICommand GotoDayCmd { get; }
        public ICommand GotoCalendarCmd { get; }
        public ICommand GotoItemCmd { get; }

        protected async override Task LoadAction() {
            await dbChecker.EnsureMigrated();
            CurrentVm = scope.Resolve<ICalendarVm>();
        }

        public void GoToDay(TodoDay day) {
            CurrentVm = scope.Resolve<IDayVm>(new TypedParameter(typeof(TodoDay), day));
        }

        void GoToCalendar(TodoDay day) {
            CurrentVm = scope.Resolve<ICalendarVm>(new TypedParameter(typeof(TodoDay), day));
        }

        public void GoToItem(TodoItem item) {
            CurrentVm = scope.Resolve<IItemVm>(new TypedParameter(typeof(TodoItem), item));
        }
    }
}
