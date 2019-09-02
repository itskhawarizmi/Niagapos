using Niagapos.Core;
using System.Windows.Controls;
using System.Linq;
using System.Data.Entity;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();


        }
      
    }
}
