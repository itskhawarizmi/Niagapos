using Pospedia.Core;
using System.Security;
using System.Windows.Controls;

namespace Pospedia
{
    /// <summary>
    /// Interaction logic for MailDeliveryControl.xaml
    /// </summary>
    public partial class MailDeliveryControl : UserControl
    {
        #region Constructor
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MailDeliveryControl()
        {
            InitializeComponent();

            DataContext = IoC.MailDelivery;

        }

        #endregion

       
       
    }
}
