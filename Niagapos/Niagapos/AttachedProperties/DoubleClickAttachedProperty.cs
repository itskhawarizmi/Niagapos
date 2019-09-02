using System;
using winForm = System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Niagapos
{
    public static class DoubleClickAttachedProperty
    {
        #region Private Members

        public static readonly DependencyProperty ClickWaitTimer = DependencyProperty.RegisterAttached("Timer", typeof(DispatcherTimer), typeof(DoubleClickAttachedProperty));

        private static DispatcherTimer GetClickWaitTimer(DependencyObject obj)
        {
            return (DispatcherTimer)obj.GetValue(ClickWaitTimer);
        }

        private static void SetClickWaitTimer(DependencyObject obj, DispatcherTimer timer)
        {
            obj.SetValue(ClickWaitTimer, timer);
        }

        #endregion

        #region Single Click Dependency Properties

        public static ICommand GetSingleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SingleClickCommand);
        }

        public static void SetSingleClickCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(SingleClickCommand, command);
        }

        public static readonly DependencyProperty SingleClickCommand = DependencyProperty.RegisterAttached("SingleClickCommand",
            typeof(ICommand), typeof(DoubleClickAttachedProperty),
            new UIPropertyMetadata(null, CommandChanged));

        public static object GetSingleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(SingleClickCommandParameter);
        }

        public static void SetSingleClickCommandParameter(DependencyObject obj, ICommand command)
        {
            obj.SetValue(SingleClickCommandParameter, command);
        }

        public static readonly DependencyProperty SingleClickCommandParameter = DependencyProperty.RegisterAttached("SingleClickCommandParameter",
            typeof(object), typeof(DoubleClickAttachedProperty));

        #endregion

        #region Double Click Dependency Properties

        public static ICommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DoubleClickCommand);
        }

        public static void SetDoubleClickCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(DoubleClickCommand, command);
        }

        public static readonly DependencyProperty DoubleClickCommand = DependencyProperty.RegisterAttached("DoubleClickCommand",
            typeof(ICommand), typeof(DoubleClickAttachedProperty),
            new UIPropertyMetadata(null, CommandChanged));

        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(DoubleClickCommandParameter);
        }

        public static void SetDoubleClickCommandParameter(DependencyObject obj, ICommand command)
        {
            obj.SetValue(DoubleClickCommandParameter, command);
        }

        public static readonly DependencyProperty DoubleClickCommandParameter = DependencyProperty.RegisterAttached("DoubleClickCommandParameter",
            typeof(object), typeof(DoubleClickAttachedProperty));

        #endregion


        public static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is UIElement targetElement))
                return;

            else
            {
                //remove any existing handlers
                targetElement.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeaveButtonDown));

                //use AddHandler to be able to listen to handled events
                targetElement.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Element_MouseLeaveButtonDown), true);

                //if the timer has not been created then do so
                var timer = GetClickWaitTimer(targetElement);

                if(timer == null)
                {
                    timer = new DispatcherTimer() { IsEnabled = false };
                    timer.Interval = new TimeSpan(0, 0, 0, 0, winForm.SystemInformation.DoubleClickTime);
                    timer.Tick += (s, args) =>
                    {
                        //if the interval has been reached without a second click then execute the SingClickCommand 
                        timer.Stop();

                        var commandParameter = targetElement.GetValue(DoubleClickCommandParameter);

                        if(targetElement.GetValue(DoubleClickCommandParameter) is ICommand command)
                        {
                            if (command.CanExecute(e))
                            {
                                command.Execute(commandParameter);
                            }
                        }
                        
                    };

                    SetClickWaitTimer(targetElement, timer);

                }
                
            }
        }

        private static void Element_MouseLeaveButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement targetElement)
            {
                var timer = GetClickWaitTimer(targetElement);

                //if the timer is enabled there has already been one click within the interval and this is a second click so 
                //stop the timer and execute the DoubleClickCommand
                if (timer.IsEnabled)
                {
                    timer.Stop();

                    var commandParameter = targetElement.GetValue(DoubleClickCommandParameter);

                    if (targetElement.GetValue(DoubleClickCommand) is ICommand command)
                    {
                        if (command.CanExecute(e))
                        {
                            command.Execute(commandParameter);
                        }
                    }
                }
                else
                {
                    timer.Start();
                }
            }
        }
    }
}
