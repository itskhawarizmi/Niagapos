/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System.Windows;

namespace Niagapos
{
    /// <summary>
    /// Details for a message box dialog 
    /// </summary>
    public class MessageBoxDialogViewModel : BaseDialogViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The teks to use for the OK button
        /// </summary>
        public string CaptionOk { get; set; } = "OK";

        /// <summary>
        /// The teks to use for the NO button
        /// </summary>
        public string CaptionNo { get; set; } = "NO";

        /// <summary>
        /// The teks to use for CANCEL button
        /// </summary>
        public string CaptionCancel { get; set; } = "CANCEL";

        /// <summary>
        /// The teks to use for YES button
        /// </summary>
        public string CaptionYes { get; set; } = "YES";

        /// <summary>
        /// The teks to use for Custom Caption button
        /// </summary>
        public string CustomCaption { get; set; } = "Okay, I know";

        /// <summary>
        /// true, if dialog OK is visible
        /// </summary>
        public bool DialogsOkVisible { get; set; }

        /// <summary>
        /// true, if dialog YES is visible
        /// </summary>
        public bool DialogsYesVisible { get; set; } = true;

        /// <summary>
        /// true, if dialog CustomCaption is visible
        /// </summary>
        public bool DialogsCustomVisible { get; set; } = true;

        /// <summary>
        /// true, if dialog NO is visible
        /// </summary>
        public bool DialogsNoVisible { get; set; }

        /// <summary>
        /// true, if dialog CANCEL is visible
        /// </summary>
        public bool DialogsCancelVisible { get; set; }

        /// <summary>
        /// Make sure we have to know what we do
        /// </summary>
        public bool IsDialogYesAction { get; set; }

        /// <summary>
        /// Make sure we have to know what we do
        /// </summary>
        public bool IsDialogNoAction { get; set; }

        /// <summary>
        /// Make sure we have to know what we do
        /// </summary>
        public bool IsDialogCancelAction { get; set; }

        /// <summary>
        /// Make sure we have to know what we do
        /// </summary>
        public bool IsDialogCustomAction { get; set; }







    }
}

