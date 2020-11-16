using System.Threading.Tasks;

namespace TodoList_sample_app.Models {
    interface INotificationDaemon {
        Task Start();
    }
}
