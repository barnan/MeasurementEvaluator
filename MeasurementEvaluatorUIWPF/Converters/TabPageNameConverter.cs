using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.Converters
{
    class TabPageNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Binding.DoNothing;
            }

            try
            {
                Page page = (Page)value;
                return page.Title;
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
