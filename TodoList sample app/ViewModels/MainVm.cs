using Autofac;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class MainVm : AAsyncLoadVm, IMainVm, INotificationReceiver, INotifyPropertyChanged {
        IDatabaseMigrator migrator;
        ILifetimeScope scope;
        INotificationDaemon notificationDaemon;

        ITodoVm currentVm;

        public MainVm(IDatabaseMigrator migrator, ILifetimeScope scope,
            INotificationDaemon notificationDaemon) {
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

        protected async override Task LoadAction() {
            await migrator.EnsureMigrated();
            await notificationDaemon.Start();

            if (currentVm == null) //could received notifs before
                CurrentVm = scope.Resolve<ICalendarVm>();
        }

        public void Receive(IEnumerable<TodoItem> items) {
            //if (currentVm is INotificationsVm currentNotificationVm) {
            //    currentNotificationVm.Add(notifs);
            //} else {
            //    CurrentVm = scope.Resolve<INotificationsVm>(
            //        new TypedParameter(typeof(IEnumerable<TodoNotification>), notifs));
            //}
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
