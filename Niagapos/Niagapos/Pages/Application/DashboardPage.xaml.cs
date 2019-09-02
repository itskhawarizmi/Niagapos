using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : BasePage<DashboardViewModel>
    {
        public DashboardPage()
        {
            InitializeComponent();

            DI.Dashboard.DataLoaded();

        }

        public DashboardPage(DashboardViewModel dashboardViewModel) : base(dashboardViewModel)
        {
            InitializeComponent();

            DI.Dashboard.DataLoaded();
        }

      
    }
}
