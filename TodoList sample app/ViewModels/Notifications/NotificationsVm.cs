using Autofac;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class NotificationsVm : ANotifyPropChanged, INotificationsVm {
        ILifetimeScope scope;
        ObservableCollection<TodoItem> notifs;

        public NotificationsVm(ILifetimeScope scope, IEnumerable<TodoItem> notifs) {
            this.scope = scope;
            Notifs = new ObservableCollection<TodoItem>(notifs);
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

        public void Add(IEnumerable<TodoItem> notifs) {

        }
    }
}
