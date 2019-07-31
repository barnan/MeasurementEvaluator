using Frame.MessageHandler;
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

            MessageSeverityLevels severity;
            try
            {
                severity = (MessageSeverityLevels)value;
            }
            catch (Exception)
            {
                return Binding.DoNothing;
            }

            switch (severity)
            {
                case MessageSeverityLevels.Trace:
                    return new SolidColorBrush(Colors.LightGray);

                case MessageSeverityLevels.Info:
                    return new SolidColorBrush(Colors.White);

                case MessageSeverityLevels.Warning:
                    return new SolidColorBrush(Colors.Orange);

                case MessageSeverityLevels.Error:
                    return new SolidColorBrush(Colors.OrangeRed);

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
