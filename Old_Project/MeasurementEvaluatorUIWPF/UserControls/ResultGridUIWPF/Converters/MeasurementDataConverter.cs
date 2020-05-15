using System;
using System.Globalization;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF.Converters
{
    public class MeasurementDataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
