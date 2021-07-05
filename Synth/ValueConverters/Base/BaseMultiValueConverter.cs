using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Synth
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">The type of this value converter</typeparam>
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
    {
        #region Private Members

        /// <summary>
        /// A single value instance of this value converter
        /// </summary>
        private static T _converter = null;

        #endregion

        #region Markup Extension Methods

        /// <summary>
        /// Provides a static instance of this converter
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        #endregion

        #region Value Converter Methods

        /// <summary>
        /// The method to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert a value back to its source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
