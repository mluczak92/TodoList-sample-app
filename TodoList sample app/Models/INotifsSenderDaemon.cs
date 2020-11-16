using System.Threading.Tasks;

namespace TodoList_sample_app.Models {
    interface INotifsSenderDaemon {
        Task Start();
    }
}
