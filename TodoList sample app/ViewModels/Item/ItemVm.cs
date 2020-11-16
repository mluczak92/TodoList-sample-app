using Autofac;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList_sample_app.Models;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    class ItemVm : AAsyncLoadVm, IItemVm {
        TodoItem item;
        IItemsRepository itemsRepo;
        ILifetimeScope scope;

        public ItemVm(TodoItem item, IItemsRepository itemsRepo, ILifetimeScope scope) {
            this.item = item;
            this.itemsRepo = itemsRepo;
            this.scope = scope;

            SaveCmd = new DelegateCommand(Save, CanSave);
            DeleteCmd = new DelegateCommand(Delete, () => true);
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


        public ICommand SaveCmd { get; }
        public ICommand DeleteCmd { get; }

        protected async override Task LoadAction() {
            Item = await itemsRepo.Refresh(item);
        }

        async void Save() {
            await itemsRepo.Update(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }

        bool CanSave() {
            return true;
        }

        async void Delete() {
            await itemsRepo.Remove(item);
            scope.Resolve<IMainVm>().GoToDay(item.Day);
        }
    }
}
