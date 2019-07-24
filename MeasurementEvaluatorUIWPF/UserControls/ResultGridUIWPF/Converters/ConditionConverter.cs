using Interfaces.Result;
using Interfaces.ToolSpecifications;
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace MeasurementEvaluatorUIWPF.UserControls.ResultGridUIWPF.Converters
{
    public class ConditionConverter : IValueConverter
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
            if (!(conditionEvalResult.Condition is ICondition<double> conditionBase))
            {
                return Binding.DoNothing;
            }
            StringBuilder sb = new StringBuilder();




        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
