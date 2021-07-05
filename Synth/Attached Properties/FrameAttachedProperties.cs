using System.Windows;
using System.Windows.Controls;

namespace Synth
{
    /// <summary>
    /// The NoFrameHistory attached property for creating a <see cref="Frame"/> that never shows navigation and doesn't keep navigation history
    /// </summary>
    public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the frame
            Frame frame = sender as Frame;

            //Hide navigation bar
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            //Clear history on navigate
            frame.Navigated += (ss, ee) => (ss as Frame).NavigationService.RemoveBackEntry();
        }
    }
}
