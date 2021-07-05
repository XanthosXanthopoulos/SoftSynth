using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for EnvelopeControl.xaml
    /// </summary>
    public partial class EnvelopeControl : UserControl
    {
        public string HoveredName
        {
            get => (string)GetValue(HoveredNameProperty);
            set => SetValue(HoveredNameProperty, value);

        }

        public static readonly DependencyProperty HoveredNameProperty = DependencyProperty.Register("HoveredName", typeof(string), typeof(EnvelopeControl), new PropertyMetadata(OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine(e.OldValue + " " + e.NewValue);
        }

        public EnvelopeControl()
        {
            InitializeComponent();
        }
    }
}
