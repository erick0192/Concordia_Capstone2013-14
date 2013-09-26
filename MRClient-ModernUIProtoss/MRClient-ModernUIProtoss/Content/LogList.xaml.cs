using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;

namespace MRClient_ModernUIProtoss.Content
{
    /// <summary>
    /// Interaction logic for LogList.xaml
    /// </summary>
    public partial class LogList : UserControl
    {
        public LogList()
        {
            InitializeComponent();
            DataContext = new LogListViewModel();
        }

        private void logListView_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            UpdateColumnWidths();
        }

        public void UpdateColumnWidths()
        {
            GridView gridView = this.logListGridView;
            if (gridView != null)
            {
                foreach (var column in gridView.Columns)
                {
                    if (double.IsNaN(column.Width))
                        column.Width = column.ActualWidth;
                    column.Width = double.NaN;
                }
            } 
        }       
    }
}
