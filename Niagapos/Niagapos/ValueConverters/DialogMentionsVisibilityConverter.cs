/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System;
using System.Globalization;
using System.Windows;

namespace Niagapos
{
    public class DialogMentionsVisibilityConverter : BaseValueConverter<DialogMentionsVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
            else
                return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

