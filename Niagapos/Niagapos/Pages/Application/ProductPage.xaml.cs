using Niagapos.Core;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class ProductPage : BasePage<ProductViewModel>
    {
        public ProductPage()
        {
            InitializeComponent();
        }

        public ProductPage(ProductViewModel productViewModel) : base(productViewModel)
        {
            InitializeComponent();
        }
    }
}
