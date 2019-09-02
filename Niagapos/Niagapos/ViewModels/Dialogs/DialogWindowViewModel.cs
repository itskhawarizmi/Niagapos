/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System.Windows;
using System.Windows.Controls;

namespace Niagapos
{
    /// <summary>
    /// The view model for the custom flat window
    /// </summary>
    public class DialogWindowViewModel : WindowViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DialogWindowViewModel(Window window) : base(window)
        {
            WindowMinimumWidth = 200;
            WindowMinimumHeight = 100;

            TitleHeight = 35;

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The title of the dialog window
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content to host inside the dialog
        /// </summary>
        public Control Content { get; set; }

        #endregion
    }
}

