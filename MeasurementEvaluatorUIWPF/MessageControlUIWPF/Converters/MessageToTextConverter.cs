using System;
using System.Globalization;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.MessageControlUI.Converters
{
    public class MessageToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Binding.DoNothing;
            }

            Message msg;
            try
            {
                msg = (Message)value;
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }

            return msg?.MessageText ?? Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
