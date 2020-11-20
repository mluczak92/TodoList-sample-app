using System.Threading.Tasks;

namespace TodoList_sample_app.Models {
    public interface INotifsSenderDaemon {
        Task Start();
    }
}
