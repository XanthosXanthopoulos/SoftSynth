using System;
using System.Windows;

namespace Synth
{
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property</typeparam>
    /// <typeparam name="Property">The type of this attached property</typeparam>
    public abstract class BaseAttachedProperty<Parent, Property> where Parent : BaseAttachedProperty<Parent, Property>, new()
    {
        #region Public Events

        /// <summary>
        /// Fired when the value changes
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, e) => { };

        /// <summary>
        /// Fired when the value changes, even if it is the same value
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated = (sender, baseValue) => { };

        #endregion

        #region Public Properties

        /// <summary>
        /// A singleton instance of the parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region Attached Property Definitions

        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(Property), typeof(BaseAttachedProperty<Parent, Property>), new PropertyMetadata(default(Property), new PropertyChangedCallback(OnValuePropertyChanged), new CoerceValueCallback(OnValuePropertyUpdated)));

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that had it's property changed</param>
        /// <param name="e">The arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the parent function
            Instance.OnValueChanged(d, e);

            //Call event listeners
            Instance.ValueChanged(d, e);
        }

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed, even if it is the same value
        /// </summary>
        /// <param name="d">The UI element that had it's property changed</param>
        /// <param name="baseValue">The value of the property</param>
        /// <returns></returns>
        private static object OnValuePropertyUpdated(DependencyObject d, object baseValue)
        {
            //Call the parent function
            Instance.OnValueUpdated(d, baseValue);

            //Call event listeners
            Instance.ValueUpdated(d, baseValue);

            //Return the value
            return baseValue;
        }

        /// <summary>
        /// Gets the attached property
        /// </summary>
        /// <param name="d">The UI element to get the property from</param>
        /// <returns></returns>
        public static Property GetValue(DependencyObject d) => (Property)d.GetValue(ValueProperty);

        /// <summary>
        /// Sets the attached property
        /// </summary>
        /// <param name="d">The UI element to set the property to</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetValue(DependencyObject d, Property value) => d.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The method that is called when any attached property of this type is changed
        /// </summary>
        /// <param name="d">The UI element tha this property was changed for</param>
        /// <param name="e">The argument for this event</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// The method that is called when any attached property of this type is changed, even if it is the same value
        /// </summary>
        /// <param name="d">The UI element tha this property was changed for</param>
        /// <param name="baseValue">The value of the property</param>
        public virtual void OnValueUpdated(DependencyObject sender, object baseValue) { }

        #endregion
    }
}
