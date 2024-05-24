using System.Windows.Controls;
using System.Windows.Input;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : UserControl
    {
        public StockView()
        {
            InitializeComponent();
        }

        private void StockQuantityBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void StockPurchasePriceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }
    }
}
