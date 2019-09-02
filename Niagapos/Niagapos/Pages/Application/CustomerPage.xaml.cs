using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : BasePage<CustomerViewModel>
    {
        public CustomerPage()
        {
            InitializeComponent();
        }

        public CustomerPage(CustomerViewModel customerViewModel) : base(customerViewModel)
        {
            InitializeComponent();
        }

    }
}
