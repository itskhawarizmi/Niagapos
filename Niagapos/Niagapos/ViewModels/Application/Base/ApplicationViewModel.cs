/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/
using Niagapos.Core;


namespace Niagapos
{
    public class ApplicationViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Login;

        /// <summary>
        /// The view model to use for the current page when the CurrentPage changes
        /// NOTE: This's not a live up to date view model of the current page 
        ///       it is simply used to set the view model of the current page 
        ///       at the time it changes
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// True if the side menu should be shown
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;

        public bool AddProductControlMenuVisible { get; set; } = false;
        public bool AddCustomerControlMenuVisible { get; set; } = false;
        public bool AddUsersControlMenuVisible { get; set; } = false;

        public bool AddReportsControlMenuVisible { get; set; } = false;

        /// <summary>
        /// True if the user credential should be shown
        /// </summary>
        public bool UserCredentialVisible { get; set; } = false;
        


        /// <summary>
        /// Navigates to the specified page.
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // Always hide Mail delivery system if we are changing pages 

            // Set the current page
            CurrentPage = page;

            // Fire off a CurrentPage changed event
            OnPropertyChanged(nameof(CurrentPage));

            // Show side menu or not ?
            if (CurrentPage == ApplicationPage.Login)
            {
                SideMenuVisible = false;
                UserCredentialVisible = false;
            }
            else
            {
                SideMenuVisible = true;
                UserCredentialVisible = true;

            }


        }




    }
}

