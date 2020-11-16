using Autofac;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class ItemVm : AAsyncLoadVm, IItemVm {
        TodoItem original;
        TodoItem item;
        IItemsRepository itemsRepo;
        ILifetimeScope scope;

        public ItemVm(TodoItem item, IItemsRepository itemsRepo, ILifetimeScope scope) {
            original = item.ShallowCopy();
            this.item = item;
            this.itemsRepo = itemsRepo;
            this.scope = scope;

            SaveCmd = new DelegateCommand(Save, CanSave);
            DeleteCmd = new DelegateCommand(Delete, CanDelete);
        }

        public TodoItem Item {
            get {
                return item;
            }
            set {
                item = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan ItemTime {
            get {
                return item.Time;
            }
            set {
                item.Time = value;
                OnPropertyChanged();
                SaveCmd.RaiseCanExecuteChanged();
                DeleteCmd.RaiseCanExecuteChanged();
            }
        }

        public string ItemNote {
            get {
                return item.Note;
            }
            set {
                item.Note = value;
                OnPropertyChanged();
                SaveCmd.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveCmd { get; }
        public DelegateCommand DeleteCmd { get; }

        protected async override Task LoadAction() {
            Item = await itemsRepo.Refresh(item);
            SaveCmd.RaiseCanExecuteChanged();
            DeleteCmd.RaiseCanExecuteChanged();
        }

        async void Save() {
            if (!CanSave()) {
                return;
            }

            if (item.Time != original.Time) { //have to change reminder after time changed
                DateTime selectedDate = item.Day.Day;
                item.ReminderTime = selectedDate.Add(item.Time.Add(TimeSpan.FromMinutes(-30)));
            }

            await itemsRepo.Update(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }

        bool CanSave() {
            return item.Day?.Day >= DateTime.Now.Date
                && item.Time >= DateTime.Now.TimeOfDay
                && !item.Equals(original);
        }

        async void Delete() {
            if (!CanDelete()) {
                return;
            }

            await itemsRepo.Remove(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }

        bool CanDelete() {
            return item.Day?.Day >= DateTime.Now.Date
                && item.Time >= DateTime.Now.TimeOfDay;
        }
    }
}
