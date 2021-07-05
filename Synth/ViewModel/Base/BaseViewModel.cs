using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;

namespace Synth
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged, INotifyActiveChanged
    {
        /// <summary>
        /// The event that is fired when any child property changes its value 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public event ActiveChangedEventHandler ActiveChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                OnPropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, args);
            }
        }

        public void OnActiveChanged(string name, double value, int id, ActiveComponent component =  ActiveComponent.Frequency)
        {
            ActiveChanged?.Invoke(this, new ActiveChangedEventArgs(name, value, id, component));
        }

        #region Command Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set. Resets the flag when the action is finished
        /// </summary>
        /// <param name="updatingFlag">The boolean property flag defining if the command is already running</param>
        /// <param name="action">The action to run if the command is not already runinng</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            //Check if the flag property is true
            if (updatingFlag.GetPropertyValue()) return;

            //Set the property flag to true
            updatingFlag.SetPropertyValue(true);

            try
            {
                //Run the passed action
                await action();
            }
            finally
            {
                //Set the property flag to false
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion
    }
}
