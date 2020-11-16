using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TodoList_sample_app.Views {
    public partial class DayUc : UserControl {
        public DayUc() {
            InitializeComponent();
        }

        private void UniformGrid_Loaded(object sender, RoutedEventArgs e) {
            UniformGrid grid = (UniformGrid)sender;
            if (grid.Children.Count == 1) {
                grid.Columns = 2;
                grid.Rows = 2;
            }
        }
    }
}
