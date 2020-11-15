using Autofac;
using Prism.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class ItemVm : IItemVm, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        TodoItem item;
        IItemsRepository itemsRepo;
        ILifetimeScope scope;

        public ItemVm(TodoItem item, IItemsRepository itemsRepo, ILifetimeScope scope) {
            this.item = item;
            this.itemsRepo = itemsRepo;
            this.scope = scope;

            LoadedCbCmd = new DelegateCommand(RefreshItem, () => true);
            SaveCmd = new DelegateCommand(Save, () => true);
            DeleteCmd = new DelegateCommand(Delete, () => true);
        }

        public TodoItem Item {
            get {
                return item;
            } set {
                item = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadedCbCmd { get; }
        public ICommand SaveCmd { get; }
        public ICommand DeleteCmd { get; }

        void RefreshItem() {
            Item = itemsRepo.ReloadItem(item);
        }

        async void Save() {
            Item = await itemsRepo.Update(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }

        async void Delete() {
            await itemsRepo.Remove(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
