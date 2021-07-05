using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Synth
{
	/// <summary>
	/// Interaction logic for KnobControl.xaml
	/// </summary>
	public partial class KnobControl : UserControl
	{
        #region Public Properties

        public bool Hovered
        {
            get { return (bool)GetValue(HoveredProperty); }
            set { SetValue(HoveredProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public int Value
        {
            get 
            {
                return Logarithmic ? (int)Math.Pow(Math.E, _a * (int)GetValue(ValueProperty) + _b) : (int)GetValue(ValueProperty);
            }
            
            set 
            {
                int val = Logarithmic ? (int)((Math.Log(value) - _b) / _a) : value;

                SetValue(ValueProperty, val);
            }
        }

        public bool Logarithmic { get; set; }

        public int DefaultValue { get; set; }

        public int Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                UpdateAB();
            }
        }

        public int Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                UpdateAB();
            }
        }

        public int MouseSpeed { get; set; }

        public int WheelStep { get; set; }

        public bool Bidirectional
		{
            get
            {
                return bidirectional;
            } 

            set
			{
                if (bidirectional == value) return;

                bidirectional = value;

                if (!IsLoaded) return;
                UpdateView();
            }
        }

        public double ArcRange
        {
            get
            {
                return arcRange;
            }

            set
            {
                if (arcRange == value) return;

                arcRange = value;
                if (!IsLoaded) return;
                UpdateView();
            }
        }

        public int LinearValue;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(KnobControl));

        public static readonly DependencyProperty HoveredProperty = DependencyProperty.Register("Hovered", typeof(bool), typeof(KnobControl), new PropertyMetadata(false));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(KnobControl), new FrameworkPropertyMetadata(OnValueChanged));

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            KnobControl knob = d as KnobControl;
            knob.UpdateView();
		}

		#endregion

		#region Constructor

		public KnobControl()
		{
			InitializeComponent();

            ArcRange = 240;
            Minimum = 1;
            Maximum = 100;
            MouseSpeed = 500;
            WheelStep = 1;
            DefaultValue = 1;
            Bidirectional = false;
            Logarithmic = false;
            UpdateAB();
            _isMouseDown = false;
        }

        #endregion

        #region Private Members

        private int minimum;
        private int maximum;
        private double _a, _b;

        private bool _isMouseDown;
        private Point _previousMousePosition;

        private bool bidirectional;
        private double arcRange;

        private double mouseMoveTotal;

        #endregion

        private void UpdateView()
        {
            int value = LinearValue;

            double angle = arcRange / (Maximum - Minimum) * (value - Minimum) - ArcRange / 2;

            if (Bidirectional)
            {
                Pointer.RenderTransform = new RotateTransform(angle, 0, 40);

                double x = 86 * Math.Sin(angle / 180.0 * Math.PI);
                double y = -86 * Math.Cos(angle / 180.0 * Math.PI);

                ArcStart.StartPoint = new Point(0 + 120, -86 + 120);

                if (angle > 0)
                {
                    ArcStop.SweepDirection = SweepDirection.Clockwise;
                }
                else
                {
                    ArcStop.SweepDirection = SweepDirection.Counterclockwise;
                }

                ArcStop.IsLargeArc = false;
                ArcStop.Point = new Point(x + 120, y + 120);
            }
            else
            {
                ArcStart.StartPoint = new Point(86 * Math.Sin(-arcRange / 2 / 180.0 * Math.PI) + 120, -86 * Math.Cos(-arcRange / 2 / 180.0 * Math.PI) + 120);

                Pointer.RenderTransform = new RotateTransform(angle, 0, 40);

                double x = 86 * Math.Sin(angle / 180.0 * Math.PI);
                double y = -86 * Math.Cos(angle / 180.0 * Math.PI);

                ArcStop.SweepDirection = SweepDirection.Clockwise;

                if (angle + arcRange / 2 >= 180)
				{
                    ArcStop.IsLargeArc = true;
                }
                else
				{
                    ArcStop.IsLargeArc = false;
                }

                ArcStop.Point = new Point(x + 120, y + 120);
            }
        }

        private void UpdateAB()
        {
            _a = Math.Log(1f * maximum / minimum) / (maximum - minimum);
            _b = Math.Log(maximum) - maximum * _a;
        }

        private void Ellipse_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var d = e.Delta / 120; // Mouse wheel 1 click (120 delta) = 1 step
            LinearValue = Math.Min(Math.Max(minimum, LinearValue + d * WheelStep), maximum);

            SetValue(ValueProperty, Logarithmic ? (int)Math.Pow(Math.E, _a * LinearValue + _b) : LinearValue);
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            (sender as Ellipse)?.CaptureMouse();
            _previousMousePosition = e.GetPosition((Ellipse)sender);
            mouseMoveTotal = 0;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown) return;

            var newMousePosition = e.GetPosition((Ellipse)sender);
            var dY = (_previousMousePosition.Y - newMousePosition.Y);
            mouseMoveTotal += dY * (maximum - minimum) / MouseSpeed;

            if (Math.Abs(mouseMoveTotal) > 1)
			{
                LinearValue = Math.Min(Math.Max(minimum, LinearValue + (int)mouseMoveTotal), maximum);
                SetValue(ValueProperty, Logarithmic ? (int)Math.Pow(Math.E, _a * LinearValue + _b) : LinearValue);
                mouseMoveTotal = mouseMoveTotal % 1;
            }

            _previousMousePosition = newMousePosition;
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            (sender as Ellipse)?.ReleaseMouseCapture();
        }

        private void KnobUserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LinearValue = DefaultValue;
            Value = DefaultValue;
        }

        private void uc_MouseEnter(object sender, MouseEventArgs e)
        {
            ((KnobControl)sender).GetBindingExpression(ValueProperty).UpdateSource();
        }

        private void KnobUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LinearValue = (int)GetValue(ValueProperty);
            UpdateView();
        }
    }
}
