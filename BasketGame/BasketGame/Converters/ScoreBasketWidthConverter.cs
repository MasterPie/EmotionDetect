// -----------------------------------------------------------------------
// <copyright file="ScoreBasketWidthConverter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ValueConversion(typeof(int),typeof(double))]
    public class ScoreBasketWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val = (int)value;
            double max = Double.Parse((string)parameter);

            return (((double)val * 1200) / max); //1010
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
