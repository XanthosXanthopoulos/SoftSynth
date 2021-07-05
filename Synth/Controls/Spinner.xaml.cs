using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Synth
{
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Gets or sets the <see cref="Label"/> of the <see cref="Spinner"/>.
        /// </summary>
        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }

            set
            {
                SetValue(LabelProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Spinner));

        /// <summary>
        /// Gets or sets the <see cref="Unit"/> of the <see cref="Spinner"/>.
        /// </summary>
        public string Unit
        {
            get
            {
                return (string)GetValue(UnitProperty);
            }

            set
            {
                SetValue(UnitProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Unit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(Spinner));

        /// <summary>
        /// Gets or sets the <see cref="Value"/> of the <see cref="Spinner"/>.
        /// </summary>
        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Spinner), new FrameworkPropertyMetadata(-1, OnValueChanged));

        /// <summary>
        /// Invoked when the effective value of the <see cref="Value"/> of this <see cref="Spinner"/> has changed.
        /// </summary>
        /// <param name="d">The <see cref="Spinner"/> whose <see cref="Value"/> has changed. </param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventHandler"/> arguments. </param>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Spinner)d).UpdateView();
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Spinner()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update the <see cref="Spinner"/> value depending on mouse wheel movement.
        /// </summary>
        /// <param name="sender">The <see cref="TextBlock"/> which fired the <see cref="MouseWheelEventHandler"/>. </param>
        /// <param name="e">The <see cref="MouseWheelEventHandler"/> arguments. </param>
        private void Ellipse_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var d = e.Delta / 120;

            int newValue = Value + d * Utilities.Pow(10, int.Parse((sender as TextBlock).Name.Substring(4)));

            Value = newValue > Math.Pow(10, MaximumDigits) - 1 || newValue < 0 ? Value : newValue;
        }

        /// <summary>
        /// Update the control view according to <see cref="Spinner"/> value.
        /// </summary>
        private void UpdateView()
        {
            int v = Value;
            int d = Utilities.Pow(10, container.Children.Count - 3);
            int m = Utilities.Pow(10, container.Children.Count - 2);

            while (v / d == 0 && container.Children.Count - 2 > MinimumDigits)
            {
                container.Children[0].MouseWheel -= Ellipse_MouseWheel;
                container.Children.RemoveAt(0);
                d /= 10;
                m /= 10;
            }

            if (v / d > 9)
            {
                container.Children.Insert(0, new TextBlock
                {
                    Name = "Part" + (container.Children.Count - 2),
                    Width = 11,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 20,
                    TextAlignment = TextAlignment.Left
                });

                container.Children[0].MouseWheel += Ellipse_MouseWheel;
                d *= 10;
                m *= 10;
            }

            foreach (TextBlock digit in container.Children)
            {
                if (regex.IsMatch(digit.Name)) continue;

                digit.Text = (v % m / d).ToString();

                d /= 10;
                m /= 10;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// The regex to filter the digit <see cref="TextBlock"/> from the symbols.
        /// </summary>
        private static Regex regex = new Regex("dot|unit");

        /// <summary>
        /// The minimum number of digits the <see cref="Spinner"/> value can have.
        /// </summary>
        private int MinimumDigits = 5;

        /// <summary>
        /// The maximum number of digits the <see cref="Spinner"/> value can have.
        /// </summary>
        private int MaximumDigits = 8;

        #endregion
    }
}
