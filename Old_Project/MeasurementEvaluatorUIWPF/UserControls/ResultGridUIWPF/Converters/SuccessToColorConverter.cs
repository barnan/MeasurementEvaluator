using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF.Converters
{
    public class SuccessToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                return Binding.DoNothing;
            }

            return boolValue ? Brushes.LightGreen : Brushes.Red;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
