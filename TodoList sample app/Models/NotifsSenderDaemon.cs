using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    class NotifsSenderDaemon : INotifsSenderDaemon {
        ILifetimeScope scope;
        IItemsRepository itemsRepository;
        DispatcherTimer dispatcherTimer;

        public NotifsSenderDaemon(ILifetimeScope scope, IItemsRepository itemsRepository) {
            this.scope = scope;
            this.itemsRepository = itemsRepository;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
        }

        public async Task Start() {
            dispatcherTimer.Start();
            await Tick();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Tick();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        async Task Tick() {
            IEnumerable<TodoItem> items = await GetItemsToNotifAbout();

            if (items.Any()) {
                NotifyReceivers(items);
            }
        }

        async Task<IEnumerable<TodoItem>> GetItemsToNotifAbout() {
            return await itemsRepository.GetOrderedItems(x => x.Day.Day.Date <= DateTime.Now.Date
                && x.ReminderTime <= DateTime.Now);
        }

        void NotifyReceivers(IEnumerable<TodoItem> items) {
            INotifsReceiver receiver = scope.Resolve<INotifsReceiver>();
            receiver.Receive(items);
        }
    }
}
