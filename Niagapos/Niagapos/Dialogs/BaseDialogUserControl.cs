/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using Niagapos.Core;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Niagapos
{
    public class BaseDialogUserControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseDialogUserControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                mDialogWindow = new DialogWindow();
                mDialogWindow.ViewModel = new DialogWindowViewModel(mDialogWindow);
                CloseCommand = new RelayCommand(action: () => mDialogWindow.Close());
               
               
            }

            IsDialogsResultCommand = new DelegateCommand<string>(IsDialogsResult);

        }

        #endregion

        #region Private Members

        /// <summary>
        /// The dialog window we will be contained within
        /// </summary>
        private DialogWindow mDialogWindow;

        #endregion

        #region Public Commands

        /// <summary>
        /// Close the dialog window
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        public ICommand IsDialogsResultCommand { get; private set; }
   

        #endregion


        public async void IsDialogsResult(string caption)
        {
            switch(caption)
            {
                case ("YES"):
                    mDialogWindow.Close();
                    break;

                case ("NO"):
                    mDialogWindow.Close();
                    break;

                case ("CANCEL"):
                    mDialogWindow.Close();
                    break;

            }

            await Task.Delay(1);

        }
        

    

        

        #region Public Properties

        /// <summary>
        /// The title of the dialog window
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The title height dialog window
        /// </summary>
        public int TitleHeight { get; set; } = 25;

        /// <summary>
        /// The minimum height of the dialog window
        /// </summary>
        public int WindowDialogMinimumHeight { get; set; }

        /// <summary>
        /// The minimum width of the dialog window
        /// </summary>
        public int WindowDialogMinimumWidth { get; set; }

        #endregion

        #region Public Dialog Show Methods

        /// <summary>
        /// Displays a single message box to the user
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <typeparam name="T">The view model type for this control</typeparam>
        /// <returns></returns>
        public Task ShowDialog<T>(T viewModel)
            where T : BaseDialogViewModel
        {
            // Create a task to await the dialog closing
            var tcs = new TaskCompletionSource<bool>();

            // Run on UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    // Match controls expected sizes to the dialog windows view model
                    mDialogWindow.ViewModel.WindowMinimumHeight = WindowDialogMinimumHeight;
                    mDialogWindow.ViewModel.WindowMinimumWidth = WindowDialogMinimumWidth;
                    mDialogWindow.ViewModel.TitleHeight = TitleHeight;
                    mDialogWindow.ViewModel.Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title;


                    // Set this control to the dialog window content
                    mDialogWindow.ViewModel.Content = this;

                    // Setup this controls data context binding to the view model
                    DataContext = viewModel;

                    // Show in the center of the parent
                    mDialogWindow.Owner = Application.Current.MainWindow;
                    mDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                    // Show dialog
                    mDialogWindow.ShowDialog();

          


                }

                finally
                {
                    // Let caller know we finished
                    tcs.TrySetResult(true);
                }
            });

            return tcs.Task;
        }

        #endregion


    }

    
    
}

