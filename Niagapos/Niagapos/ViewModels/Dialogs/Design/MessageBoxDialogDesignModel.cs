/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


namespace Niagapos
{
    /// <summary>
    /// Details for a message box dialog
    /// </summary>
    public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel
    {
        #region Singleton

        /// <summary>
        /// A single instance of the design model
        /// </summary>
        public static MessageBoxDialogDesignModel Instance = new MessageBoxDialogDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MessageBoxDialogDesignModel()
        {
            CaptionOk = "OK";
            CaptionNo = "NO";
            CaptionYes = "YES";
            CaptionCancel = "CANCEL";
            Message = "Hello World!";
        }

        #endregion


    }
}

