using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.ViewModels {
    public interface IMainVm {
        ITodoVm CurrentVm { get; }
        void GoToDay(TodoDay day);
        void GotoDayCanExecuteChanged();
        void GoToItem(TodoItem item);
        void CloseNotif();
    }
}
