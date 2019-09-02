using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class ReportPage : BasePage<ReportViewModel>
    {
        public ReportPage()
        {
            InitializeComponent();
        }

        public ReportPage(ReportViewModel reportViewModel) : base(reportViewModel)
        {
            InitializeComponent();
        }

    }
}
