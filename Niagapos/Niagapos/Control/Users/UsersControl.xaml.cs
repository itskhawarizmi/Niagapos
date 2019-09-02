using Niagapos.Core;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class UsersControl : UserControl, IHavePassword
    {
        public UsersControl()
        {
            InitializeComponent();

            DataContext = new UserViewModel();

          
        }
        
        public SecureString SecurePassword => PasswordText.SecurePassword;
    }
}
