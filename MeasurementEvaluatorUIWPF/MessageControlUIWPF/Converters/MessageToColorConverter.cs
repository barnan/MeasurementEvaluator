using Interfaces;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MeasurementEvaluatorUIWPF.MessageControlUI.Converters
{
    public class MessageToColorConverter : IValueConverter
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

            switch (msg.MessageSeverityLevel)
            {
                case MessageSeverityLevels.Trace:
                    return Colors.White;
                case MessageSeverityLevels.Info:
                    return Colors.Black;

                case MessageSeverityLevels.Warning:
                    return Colors.Orange;

                case MessageSeverityLevels.Error:
                    return Colors.DarkRed;
                default:
                    return Binding.DoNothing;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
