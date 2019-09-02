using Pospedia.Core;
using System.Windows.Controls;

namespace Pospedia
{
    /// <summary>
    /// Interaction logic for ProductsControl.xaml
    /// </summary>
    public partial class ProductsControl : UserControl
    {
        public ProductsControl()
        {
            InitializeComponent();

            DataContext = IoC.Product;
        }

    }
}
