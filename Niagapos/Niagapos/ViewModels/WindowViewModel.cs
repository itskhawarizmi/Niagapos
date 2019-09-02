/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/

using Niagapos.Core;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Niagapos
{
    public class WindowViewModel : BaseViewModel
    {

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            mWindow.StateChanged += (sender, e) =>
            {
                WindowResized();
            };

            // Create Commands
            WindowMinimizeCommand = new RelayCommand(action: () => mWindow.WindowState = WindowState.Minimized);
            WindowMaximizeCommand = new RelayCommand(action: () => mWindow.WindowState ^= WindowState.Maximized);
            WindowCloseCommand = new RelayCommand(action: () => mWindow.Close());
            WindowMenuCommand = new RelayCommand(action: () => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            mWindowResizer = new WindowResizer(mWindow);

            mWindowResizer.WindowDockChanged += (dock) =>
            {
                mDockPosition = dock;
                WindowResized();
            };

            DateTimeRealTime();



        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel()
        {

        }

        #endregion

        #region Private Helpers

        private Point GetMousePosition()
        {
            var position = Mouse.GetPosition(mWindow);

            if (mWindow.WindowState == WindowState.Maximized)
                return new Point(position.X + mWindowResizer.CurrentMonitorSize.Left, position.Y + mWindowResizer.CurrentMonitorSize.Top);
            else
                return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);



        }

        private void DateTimeRealTime()
        {
            //var timer = new DispatcherTimer
            //{
            //    Interval = TimeSpan.FromSeconds(1)
            //};

            //timer.Tick += (s, e) =>
            //{

            //    DateTimeNow = DateTime.Now.ToString();

            //};

            //timer.Start();
        }

        private void WindowResized()
        {
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(OuterMarginSize));
        }

        #endregion

        #region Private Members

        private WindowResizer mWindowResizer;

        private Window mWindow;

        private int mOuterMarginSize = 10;

        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Properties
        

        public string DateTimeNow { get; set; }
        

        public int WindowMinimumHeight { get; set; } = 500;

        public int WindowMinimumWidth { get; set; } = 800;

        public int TitleHeight { get; set; } = 30;

        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        public int ResizeBorder => 10;

        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + mOuterMarginSize);

        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        public int OuterMarginSize { get => Borderless ? 0 : mOuterMarginSize; set => mOuterMarginSize = value; }

        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        public GridLength TitleHeightGridLength => new GridLength(TitleHeight);

        /// <summary>
        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        /// </summary>
        public bool DimmableOverlayVisible { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand WindowCloseCommand { get; set; }

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand WindowMinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand WindowMaximizeCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand WindowMenuCommand { get; set; }

        #endregion
    }
}

