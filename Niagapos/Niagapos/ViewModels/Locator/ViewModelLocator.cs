/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// Locates view models from the IoC for use in binding XAML files
    /// </summary>
    public class ViewModelLocator 
    {
        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();
        
        /// <summary>   
        /// The application view model
        /// </summary>
        public static ApplicationViewModel ApplicationViewModel => DI.Application;

        /// <summary>
        /// The dashboard view model
        /// </summary>
        public static DashboardViewModel DashboardViewModel => DI.Dashboard;

        /// <summary>
        /// The Sales view model
        /// </summary>
        public static TransactionViewModel TransactionViewModel => DI.Transaction;

        /// <summary>
        /// The Sales view model
        /// </summary>
        public static ProductViewModel ProductViewModel => DI.Product;

        /// <summary>
        /// The usercredential view model
        /// </summary>
        public static UserCredentialViewModel UserCredentialViewModel => DI.UserCredential;

        // <summary>
        /// The usercredential view model
        /// </summary>
        public static UserViewModel UserViewModel => DI.Users;


        // <summary>
        /// The usercredential view model
        /// </summary>
        public static ReportViewModel ReportViewModel => DI.Report;


        #endregion
    }
}

