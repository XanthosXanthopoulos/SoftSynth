using System.Windows;

namespace Synth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public object ApplicationiewModel { get; private set; }

        /// <summary>
        /// Custom startup to load the IoC immediately
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //Let the base application do what it needs
            base.OnStartup(e);

            //Setup the IoC
            IoCContainer.Setup();

            //Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
