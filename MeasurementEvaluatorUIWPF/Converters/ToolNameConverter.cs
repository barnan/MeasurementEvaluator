using Interfaces;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.Converters
{
    public class ToolNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ToolNames toolNameObj))
            {
                return Binding.DoNothing;
            }

            return toolNameObj.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
