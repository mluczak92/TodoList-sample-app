using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TodoList_sample_app.Views {
    public partial class NotificationsUc : UserControl {
        public NotificationsUc() {
            InitializeComponent();
        }

        private void UniformGrid_Loaded(object sender, RoutedEventArgs e) {
            UniformGrid ug = (UniformGrid)sender;
            if (ug.Rows < 5) {
                ug.Rows = 5;
            }
        }
    }
}
