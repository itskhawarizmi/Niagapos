using Niagapos.Core;
using System.Security;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : BasePage<UserViewModel>, IHavePassword
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        public UsersPage(UserViewModel usersViewModel) : base(usersViewModel)
        {
            InitializeComponent();
        }

        public SecureString SecurePassword => PasswordText.SecurePassword;
    }
}
