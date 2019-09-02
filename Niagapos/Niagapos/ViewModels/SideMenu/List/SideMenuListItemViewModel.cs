/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using Niagapos.Core;
using System.Windows.Input;

namespace Niagapos
{
    public class SideMenuListItemViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SideMenuListItemViewModel()
        {
            MenuPagesCommand = new RelayCommand(action: () => MenuPages());
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Name of side menu items
        /// </summary>
        public string NameMenuItems { get; set; }

        /// <summary>
        /// The icon of the menu items
        /// </summary>
        public IconType MenuIcons { get; set; }

        /// <summary>
        /// True if this item currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to move the page to the specific page
        /// </summary>
        public ICommand MenuPagesCommand { get; set; }

        #endregion

        #region Command Methods

        public void MenuPages()
        {
            switch (NameMenuItems)
            {
                case "Dashboard":
                    DI.Application.GoToPage(ApplicationPage.Dashboard);
                    DI.Application.SideMenuVisible = true;
                    break;

                case "User":
                    if (DI.UserCredential.Caption == "Administrator")
                    {
                        DI.Application.GoToPage(ApplicationPage.Users);

                    }
                    else
                    {
                        DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Message",
                            Message = $"Hi, {DI.UserCredential.Name} you must have permission to access {NameMenuItems} page!, " +
                            $"Please contact your administrator",

                        });

                        return;
                    }

                    break;

                case "Customer":
                    if (DI.UserCredential.Caption == "Administrator" ||  DI.UserCredential.Caption == "Cashier")
                    {
                        DI.Application.GoToPage(ApplicationPage.Customers);
                    }
                    else
                    {
                        DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Message",
                            Message = $"Hi, {DI.UserCredential.Name} you must have permission to access {NameMenuItems} page!, " +
                            $"Please contact your administrator",

                        });

                        return;
                    }
                    break;

                case "Produk":
                    DI.Application.GoToPage(ApplicationPage.Products);
                    DI.Application.SideMenuVisible = true;
                    break;

                case "Transaksi":
                    if (DI.UserCredential.Caption == "Administrator" || DI.UserCredential.Caption == "Cashier")
                    {
                        DI.Application.GoToPage(ApplicationPage.Transaction);
                    }
                    else
                    {
                        DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Message",
                            Message = $"Hi, {DI.UserCredential.Name} you must have permission to access {NameMenuItems} page!, " +
                            $"Please contact your administrator",

                        });

                        return;
                    }
                    break;


                case "Laporan":
                    DI.Application.GoToPage(ApplicationPage.Reports);
                    DI.Application.SideMenuVisible = true;
                    break;

                case "Logout":
                    DI.Application.GoToPage(ApplicationPage.Login);
                    DI.Application.SideMenuVisible = false;
                    break;
            }



        }




        #endregion

    }
}

