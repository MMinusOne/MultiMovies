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
    public class ServerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (EpisodeSource)value;
            if (source == null) return null;
            if (WatchPageViewModel.Instance.Source == null) return null;
            if (WatchPageViewModel.Instance.Source.server == source.server)
            {
                return Brushes.Red;
            }
            else
            {
                return Brushes.Green;
            }
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
