/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System;
using System.Windows;
using System.Windows.Input;

namespace Niagapos
{
    /// <summary>
    /// This class facilitates associating a key binding in XAML markup to a command
    /// defined in a view model by exposing a command dependency property.
    /// The class derives from freezable to work around a limitation in WPF when data binding from XAML
    /// </summary>
    public class CommandReference : Freezable, ICommand
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CommandReference() { }

        #endregion

        #region Dependency Properties

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("CommandProperty", typeof(ICommand),
                typeof(CommandReference), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandReference mCommandReference = d as CommandReference;

            if (e.NewValue is ICommand oldCommand)
                oldCommand.CanExecuteChanged += mCommandReference.CanExecuteChanged;
            if (e.OldValue is ICommand newCommand)
                newCommand.CanExecuteChanged -= mCommandReference.CanExecuteChanged;
        }

        #endregion



        #region Public Events

        /// <summary>
        /// The event thats fired when <see cref="CanExecute(object)"/> has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (s, e) => { };

        #endregion

        #region Command Methods

        public bool CanExecute(object parameter)
        {
            if (Command != null)
                return Command.CanExecute(parameter);

            return false;
        }

        public void Execute(object parameter) => Command.Execute(parameter);

        #endregion


        #region Freezable
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

