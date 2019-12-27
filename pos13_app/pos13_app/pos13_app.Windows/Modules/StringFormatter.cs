using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace pos13_app.Modules
{
    public class StringFormatter
    {
        public object Convert(object value, Type targetType, object parameter,
                                System.Globalization.CultureInfo culture)
        {
            return String.Format(culture, "Cost: {0:C}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
