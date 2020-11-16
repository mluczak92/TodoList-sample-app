using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using TodoList_sample_app.Models.Database;

namespace TodoList_sample_app.Models {
    class NotificationDaemon : INotificationDaemon {
        ILifetimeScope scope;
        IItemsRepository itemsRepository;
        DispatcherTimer dispatcherTimer;

        public NotificationDaemon(ILifetimeScope scope, IItemsRepository itemsRepository) {
            this.scope = scope;
            this.itemsRepository = itemsRepository;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMinutes(1);
        }

        public async Task Start() {
            //dispatcherTimer.Start();
            //await Tick();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //Tick();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        //        async Task Tick() {
        //            IEnumerable<TodoItem> items = await GetItemsToCreateNotif();

        //            if (items.Any()) {
        //                IEnumerable<TodoNotification> notifs = await CreateNotifs(items);
        //                NotifyReceivers(notifs);
        //            }
        //        }

        //        async Task<IEnumerable<TodoItem>> GetItemsToCreateNotif() {
        //            return await itemsRepository.GetOrderedItems(x => x.Day.Day.Date <= DateTime.Now.Date
        //                && x.Time <= DateTime.Now.AddMinutes(15).TimeOfDay
        //                && !x.Notifications.Any(x => x.IsActive));
        //        }

        //        async Task<IEnumerable<TodoNotification>> CreateNotifs(IEnumerable<TodoItem> items) {
        //            IEnumerable<TodoNotification> notifs = items.Select(x => new TodoNotification(x));
        //            await notifsRepository.AddRange(notifs);
        //            return notifs;
        //        }

        //        void NotifyReceivers(IEnumerable<TodoNotification> notifs) {
        //            INotificationReceiver receiver = scope.Resolve<INotificationReceiver>();
        //            receiver.Receive(notifs);
        //        }
    }
}
