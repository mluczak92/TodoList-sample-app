using Autofac;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class MainVm : AAsyncLoadVm, IMainVm, INotifsReceiver, INotifyPropertyChanged {
        IDatabaseMigrator migrator;
        ILifetimeScope scope;
        INotifsSenderDaemon notificationDaemon;

        ITodoVm currentVm;
        ITodoVm preNotificationVm;

        public MainVm(IDatabaseMigrator migrator, ILifetimeScope scope,
            INotifsSenderDaemon notificationDaemon) {
            this.migrator = migrator;
            this.scope = scope;
            this.notificationDaemon = notificationDaemon;

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
        public ICommand CloseNotifCmd { get; }

        protected async override Task LoadAction() {
            await migrator.EnsureMigrated();
            CurrentVm = scope.Resolve<ICalendarVm>();
            await notificationDaemon.Start();
        }

        public void Receive(IEnumerable<TodoItem> items) {
            if (!(currentVm is INotificationsVm)) {
                preNotificationVm = currentVm;
            }

            CurrentVm = scope.Resolve<INotificationsVm>(
                new TypedParameter(typeof(IEnumerable<TodoItem>), items));
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

        public void CloseNotif() {
            CurrentVm = preNotificationVm;
            preNotificationVm = null;
        }
    }
}
