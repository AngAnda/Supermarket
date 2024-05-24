using System.Windows.Controls;
using System.Windows.Input;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
        }

        private void CategoryNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z0-9]+$"))
            {
                e.Handled = true;
            }
        }
    }
}
