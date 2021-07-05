using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlarmUI
{
    partial class ListBackend : ResourceDictionary
    {
        public ListBackend()
        {
            InitializeComponent();
        }

        private void EditHeader(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Grid parent = VisualTreeHelper.GetParent(sender as TextBlock) as Grid;

                TextBox editTextBox = parent.FindName("EditTextBox") as TextBox;
                KeyboardControl keyboard = Window.GetWindow(sender as TextBlock).FindName("VirtualKeyboard") as KeyboardControl;
                keyboard.ZoneTextBox = editTextBox;

                editTextBox.Visibility = Visibility.Visible;
                Action changeCaret = () => Keyboard.Focus(editTextBox);
                (sender as TextBlock).Dispatcher.BeginInvoke(changeCaret);


                IoCContainer.Get<ApplicationViewModel>().KeyboardVisible = true;
            }
        }

        private void EditHeader_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox editTextBox = sender as TextBox;
            editTextBox.Visibility = Visibility.Hidden;
            IoCContainer.Get<ApplicationViewModel>().KeyboardVisible = false;
        }
    }
}
