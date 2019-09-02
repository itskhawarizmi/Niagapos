/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using System.Diagnostics;
using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageHelpers
    {

        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and view model, if any, and create the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page
            switch (page)
            {
                case ApplicationPage.Login:
                    return new LoginPage(viewModel as LoginViewModel);

                case ApplicationPage.Dashboard:
                    return new DashboardPage(viewModel as DashboardViewModel);

                case ApplicationPage.Products:
                    return new ProductPage(viewModel as ProductViewModel);

                case ApplicationPage.Users:
                    return new UsersPage(viewModel as UserViewModel);
                    
                case ApplicationPage.Customers:
                    return new CustomerPage(viewModel as CustomerViewModel);

                case ApplicationPage.Transaction:
                    return new TransactionPage(viewModel as TransactionViewModel);

                case ApplicationPage.Reports:
                    return new ReportPage(viewModel as ReportViewModel);

                default:
                    Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Find application page that matches the base page
            if (page is LoginPage)
                return ApplicationPage.Login;

            if (page is DashboardPage)
                return ApplicationPage.Dashboard;

            if (page is ProductPage)
                return ApplicationPage.Products;

            if (page is UsersPage)
                return ApplicationPage.Users;

            if (page is CustomerPage)
                return ApplicationPage.Customers;

            if (page is TransactionPage)
                return ApplicationPage.Transaction;

            if (page is ReportPage)
                return ApplicationPage.Reports;

            // Alert developer of issue
            Debugger.Break();
            return default(ApplicationPage);
        }
    }
}

