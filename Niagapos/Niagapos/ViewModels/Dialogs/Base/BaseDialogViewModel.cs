/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/

using Niagapos.Core;

namespace Niagapos
{
    /// <summary>
    /// A base view model for any dialogs
    /// </summary>
    public class BaseDialogViewModel : BaseViewModel
    {
        /// <summary>
        /// The title of the dialog
        /// </summary>
        public string Title { get; set; }
    }
}

