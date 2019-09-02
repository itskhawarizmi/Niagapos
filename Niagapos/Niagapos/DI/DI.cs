using Niagapos.Core;
using Ninject;

namespace Niagapos
{
    public static class DI
    {
        public static IKernel Kernel { get; set; } = new StandardKernel();

        public static IUIManager UI => DI.GetService<IUIManager>();

        public static void Setup() => BindsViewModel();

        /// <summary>
        /// A shortcut to acces the <see cref="MessageBoxDialogViewModel"/>
        /// </summary>
        public static MessageBoxDialogViewModel MessageBoxDialog => DI.GetService<MessageBoxDialogViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => DI.GetService<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="UserCredentialViewModel"/>
        /// </summary>
        public static UserCredentialViewModel UserCredential => DI.GetService<UserCredentialViewModel>();


        /// <summary>
        /// A shortcut to access the <see cref="LoginViewModel"/>
        /// </summary>
        public static LoginViewModel Login => DI.GetService<LoginViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="LoginViewModel"/>
        /// </summary>
        public static DashboardViewModel Dashboard => DI.GetService<DashboardViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="LoginViewModel"/>
        /// </summary>
        public static ProductViewModel Product => DI.GetService<ProductViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="SalesViewModel"/>
        /// </summary>
        public static TransactionViewModel Transaction => DI.GetService<TransactionViewModel>();


        /// <summary>
        /// A shortcut to access the <see cref="CustomerViewModel"/>
        /// </summary>
        public static CustomerViewModel Customer => DI.GetService<CustomerViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="CustomerViewModel"/>
        /// </summary>
        public static UserViewModel Users => DI.GetService<UserViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="CustomerViewModel"/>
        /// </summary>
        public static ReportViewModel Report => DI.GetService<ReportViewModel>();


        public static void BindsViewModel()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            Kernel.Bind<LoginViewModel>().ToConstant(new LoginViewModel());

            Kernel.Bind<DashboardViewModel>().ToConstant(new DashboardViewModel());

            Kernel.Bind<ProductViewModel>().ToConstant(new ProductViewModel());

            Kernel.Bind<TransactionViewModel>().ToConstant(new TransactionViewModel());

            Kernel.Bind<UserCredentialViewModel>().ToConstant(new UserCredentialViewModel());

            Kernel.Bind<CustomerViewModel>().ToConstant(new CustomerViewModel());

            Kernel.Bind<ReportViewModel>().ToConstant(new ReportViewModel());
        }

        public static T GetService<T>() => Kernel.Get<T>();
    }
}
