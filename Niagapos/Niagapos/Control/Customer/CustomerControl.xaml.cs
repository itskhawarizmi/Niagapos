using Niagapos.Core;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        public CustomerControl()
        {
            InitializeComponent();

            DataContext = new CustomerViewModel();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
