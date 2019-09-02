using Niagapos.Core;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for UsersCredentialControl.xaml
    /// </summary>
    public partial class UsersCredentialControl : UserControl
    {
        public UsersCredentialControl()
        {
            InitializeComponent();

            DataContext = new UserCredentialViewModel();
        }
    }
}
