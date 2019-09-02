/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using System;
using System.Globalization;

namespace Niagapos
{
    public class BooleanInvertConverter : BaseValueConverter<BooleanInvertConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

