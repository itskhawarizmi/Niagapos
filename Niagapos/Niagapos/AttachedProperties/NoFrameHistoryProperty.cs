/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using System.Windows;
using System.Windows.Controls;

namespace Niagapos
{
    public class NoFrameHistoryProperty : BaseAttachedProperty<NoFrameHistoryProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = (d as Frame);

            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            frame.Navigated += (sender, ee) => ((Frame)sender).NavigationService.RemoveBackEntry();
        }
    }
}

