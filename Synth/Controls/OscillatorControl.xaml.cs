using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Synth
{
	/// <summary>
	/// Interaction logic for OscillatorControl.xaml
	/// </summary>
	public partial class OscillatorControl : UserControl
	{
        #region Public Properties

		public static void PrintfDebugger(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Console.WriteLine("{0}.{1}: {2}",
				d.DependencyObjectType.Name,
				e.Property.Name,
				e.NewValue);
		}

	#endregion

		public OscillatorControl()
		{
			InitializeComponent();
		}

        private void uc_MouseEnter(object sender, MouseEventArgs e)
        {
			WaveBase.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
		}
    }
}
