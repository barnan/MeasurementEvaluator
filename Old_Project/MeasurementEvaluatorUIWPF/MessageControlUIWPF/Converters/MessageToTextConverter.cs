using System;
using System.Globalization;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.MessageControlUI.Converters
{
    public class MessageToTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                return Binding.DoNothing;
            }

            if (!(values[0] is string text))
            {
                return Binding.DoNothing;
            }

            if (!(values[1] is DateTime time))
            {
                return Binding.DoNothing;
            }

            string finalMessage = $"{time.ToString("yyyy.HH.dd - HH:mm:ss.fff")} - {text}";

            return finalMessage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
