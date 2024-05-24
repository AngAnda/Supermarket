using System.Windows.Controls;
using System.Windows.Input;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for ProducersView.xaml
    /// </summary>
    public partial class ProducersView : UserControl
    {
        public ProducersView()
        {
            InitializeComponent();
        }

        private void ProducerNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9]+$"))
            {
                e.Handled = true;
            }
        }

        private void ProducerCountryBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9]+$"))
            {
                e.Handled = true;
            }
        }
    }
}
