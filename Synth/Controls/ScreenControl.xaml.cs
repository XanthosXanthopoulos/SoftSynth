using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synth
{
    /// <summary>
    /// Interaction logic for ScreenControl.xaml
    /// </summary>
    public partial class ScreenControl : UserControl
    {

        public double[] WaveData
        {
            get => (double[])GetValue(WaveDataProperty);
            set => SetValue(WaveDataProperty, value);
        }

        public static readonly DependencyProperty WaveDataProperty = DependencyProperty.Register("WaveData", typeof(double[]), typeof(ScreenControl), new PropertyMetadata(OnWaveDataChanged));

        private static void OnWaveDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!((ScreenControl)d).IsLoaded) return;

            Canvas canvas = ((ScreenControl)d).WaveTable;

            canvas.Children.RemoveAt(canvas.Children.Count - 1);

            double width = (canvas.ActualWidth - 20) / 400;
            double height = canvas.ActualHeight / 2 - 20;

            double[] data = (double[])e.NewValue;

            Polyline polyline = new Polyline
            {
                Stroke = new SolidColorBrush(Color.FromRgb(111, 220, 248)),
                StrokeThickness = 1,
                Opacity = 0.8
            };

            for (int i = 0; i < data.Length; ++i)
            {
                polyline.Points.Add(new Point(10 + i * width, -data[i] * (height + 10) + height + 20));
            }

            canvas.Children.Add(polyline);
        }

        public ScreenControl()
        {
            InitializeComponent();
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsVisible) return;

            double width = WaveTable.ActualWidth - 20;
            double height = WaveTable.ActualHeight / 2 - 20;

            Polyline horizontalLine = new Polyline
            {
                Stroke = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                StrokeThickness = 0.6,
                Opacity = 0.8,
                Points = new PointCollection()
                {
                    new Point(10, height + 20),
                    new Point(width + 10, height + 20)
                }
            };

            WaveTable.Children.Add(horizontalLine);

            for (int i = -4; i < 5; ++i)
            {
                Polyline line = new Polyline
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
                    StrokeThickness = 0.6,
                    Opacity = 0.5,
                    Points = new PointCollection()
                    {
                        new Point(width / 2 + i * (width / (8)) + 10, 10),
                        new Point(width / 2 + i * (width / (8)) + 10, 2 * height + 30)
                    }
                };

                if (i == 0)
                {
                    line.Stroke = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    line.Opacity = 0.8;
                }

                WaveTable.Children.Add(line);
            }

            WaveTable.Children.Add(new Polyline());
        }
    }
}
