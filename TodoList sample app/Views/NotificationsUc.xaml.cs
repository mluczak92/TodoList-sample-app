using System;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using TodoList_sample_app.ViewModels;

namespace TodoList_sample_app.Views {
    public partial class NotificationsUc : UserControl {
        [DllImport("user32")]
        public static extern int FlashWindow(IntPtr hwnd, bool bInvert);

        public NotificationsUc() {
            InitializeComponent();
        }

        private void ItemsControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if ((e.OldValue == null && e.NewValue != null) || //first DataContext assigned
                (e.OldValue is INotificationsVm oldVm && e.NewValue is INotificationsVm newVm &&
                newVm.Notifications.Count() > oldVm.Notifications.Count())) { //or new vm with more notifs

                Window window = Window.GetWindow(this);
                WindowInteropHelper wih = new WindowInteropHelper(window);

                window.Topmost = true;
                if (window.WindowState == WindowState.Minimized) {
                    window.WindowState = WindowState.Normal;
                }

                FlashWindow(wih.Handle, true);
                SystemSounds.Exclamation.Play();
                window.Topmost = false;
            }
        }
    }
}
