namespace Synth
{
    public delegate void ActiveChangedEventHandler(object sender, ActiveChangedEventArgs e);

    public interface INotifyActiveChanged
    {
        public event ActiveChangedEventHandler ActiveChanged;
    }
}
