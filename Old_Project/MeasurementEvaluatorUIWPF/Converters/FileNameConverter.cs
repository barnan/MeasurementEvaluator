using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.Converters
{
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Path.GetFileName(value as string);
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
