using System;
using System.Diagnostics;
using System.Globalization;

namespace Synth
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.SynthPage:
                    return new OscillatorsPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
