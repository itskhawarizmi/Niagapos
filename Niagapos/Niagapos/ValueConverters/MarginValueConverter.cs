/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.xcodeplus.net  *
 ************************************/


using System;
using System.Globalization;
using System.Windows;

namespace Niagapos
{
    public class MarginValueConverter : BaseValueConverter<MarginValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Thickness thickness = new Thickness(18, 30, 5, 10);

            //Thickness marginVisible = new Thickness(15, 30, -315, 10);
            //Thickness marginHidden = new Thickness(18, 30, 5, 10);

            return new Thickness(15, 30, System.Convert.ToDouble(value), 10);


        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

