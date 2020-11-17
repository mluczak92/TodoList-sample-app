using Autofac;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class NotificationsVm : ANotifyPropChanged, INotificationsVm {
        ILifetimeScope scope;
        ObservableCollection<TodoItem> notifs;
        IItemsRepository itemsRepo;

        public NotificationsVm(ILifetimeScope scope, IEnumerable<TodoItem> notifs,
            IItemsRepository itemsRepo) {
            this.scope = scope;
            Notifs = new ObservableCollection<TodoItem>(notifs);
            this.itemsRepo = itemsRepo;

            RemindLaterCmd = new DelegateCommand<TodoItem>(RemindLater, x => true);
            DontShowCmd = new DelegateCommand<TodoItem>(DontShow, x => true);
        }

        public ObservableCollection<TodoItem> Notifs {
            get {
                return notifs;
            }
            set {
                notifs = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemindLaterCmd { get; }
        public ICommand DontShowCmd { get; }

        public void Add(IEnumerable<TodoItem> items) {
            foreach (TodoItem i in items) {
                if (notifs.Contains(i)) {
                    continue;
                }

                Notifs.Add(i);
            }
        }

        void RemindLater(TodoItem item) {
            item.ReminderTime = DateTime.Now.AddMinutes(15);
            itemsRepo.Update(item);
            Notifs.Remove(item);
            GoBackIfLast();
        }

        void DontShow(TodoItem item) {
            item.ReminderTime = null;
            itemsRepo.Update(item);
            Notifs.Remove(item);
            GoBackIfLast();
        }

        void GoBackIfLast() {
            if (!notifs.Any())
                scope.Resolve<IMainVm>().CloseNotif();
        }
    }
}
