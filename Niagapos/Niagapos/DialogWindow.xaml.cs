using System.Windows;

namespace Niagapos
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        #region Private Members

        /// <summary>
        /// The view model for this window
        /// </summary>
        private DialogWindowViewModel mViewModel;

        #endregion

        #region Public Properties

        public DialogWindowViewModel ViewModel
        {
            get => mViewModel;
            set
            {
                mViewModel = value;

                // Update DataContext
                DataContext = mViewModel;
            }
        }

        #endregion


    }
}
