using System;
using System.Windows;

namespace Niagapos
{
    public static class ExtensionMethods
    {
        public static void DoWhenLoad<T>(this T element, Action<T> action)
            where T : FrameworkElement
        {
            if (element.IsLoaded)
                action(element);

            else
            {
                void handler(object sender, RoutedEventArgs e)
                {
                    element.Loaded -= handler;
                    action(element);
                }

                element.Loaded += handler;
            }

        }
    }

}
