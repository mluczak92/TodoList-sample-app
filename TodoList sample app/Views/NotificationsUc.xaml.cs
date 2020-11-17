using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace TodoList_sample_app.Views {
    public partial class NotificationsUc : UserControl {
        [DllImport("user32")]
        public static extern int FlashWindow(IntPtr hwnd, bool bInvert);

        public NotificationsUc() {
            InitializeComponent();
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e) {
            Window window = Window.GetWindow(this);

            WindowInteropHelper wih = new WindowInteropHelper(window);
            FlashWindow(wih.Handle, true);

            if (window.WindowState == WindowState.Minimized) {
                window.WindowState = WindowState.Normal;
            }

            SystemSounds.Exclamation.Play();
        }
    }
}
