using Interfaces.Result;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF.Converters
{
    public class ConditionToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Binding.DoNothing;
            }
            if (!(value is IConditionEvaluationResult conditionEvalResult))
            {
                return Binding.DoNothing;
            }

            return conditionEvalResult.ToString("grid", null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
