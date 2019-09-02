/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System;
using System.Globalization;

namespace Niagapos
{
    public class ThumbnailValueConverter : BaseValueConverter<ThumbnailValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (Uri)(new UriTypeConverter().ConvertFrom($"{value}"));

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {   
            return (Uri)(new UriTypeConverter().ConvertFrom($"{value}"));
        }
    }
}

