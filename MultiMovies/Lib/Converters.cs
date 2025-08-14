using MultiMovies.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MultiMovies.Lib.Converters
{

    public class DurationMSToDisplayableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return "00:00:00";
            var durationMS = (long)value; 
            TimeSpan timespan = TimeSpan.FromMilliseconds(durationMS);

            return string.Format("{0:D2}:{1:D2}:{2:D2}", 
                (int)timespan.TotalHours, 
                timespan.Minutes, 
                timespan.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TruncateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return null;

            int maxChars = 450;
            if (parameter != null && int.TryParse(parameter.ToString(), out int p))
                maxChars = p;

            if (str.Length <= maxChars) return str;
            return str.Substring(0, maxChars) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
