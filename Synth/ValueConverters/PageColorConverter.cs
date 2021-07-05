using System;
using System.Globalization;

namespace Synth
{
    class PageColorConverter : BaseValueConverter<PageColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationPage currentPage = (ApplicationPage)value;
            ApplicationPage buttonPage = (ApplicationPage)parameter;

            if (currentPage == buttonPage)
            {
				return System.Windows.Application.Current.FindResource("GrayElevetion01dpBrush");
            }
            else
            {
				return System.Windows.Application.Current.FindResource("GrayElevetion00dpBrush");
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
