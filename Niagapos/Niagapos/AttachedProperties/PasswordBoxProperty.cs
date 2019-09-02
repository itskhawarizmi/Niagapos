/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using System.Windows;
using System.Windows.Controls;

namespace Niagapos
{
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (!(d is PasswordBox passwordBox))
                return;

            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if ((bool)e.NewValue)
            {
                HasTextProperty.SetValue(passwordBox);

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => 
            HasTextProperty.SetValue((PasswordBox)sender);
    }


    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        public static void SetValue(DependencyObject sender) => 
            SetValue(d: sender, value: ((PasswordBox)sender).SecurePassword.Length > 0);
    }
}

