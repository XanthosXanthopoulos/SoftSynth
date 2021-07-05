using System;

namespace Synth
{
    public class ActiveChangedEventArgs : EventArgs
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public int Id { get; set; }

        public ActiveComponent Component { get; set; }

        public ActiveChangedEventArgs(string name, double value, int id, ActiveComponent component = ActiveComponent.Frequency)
        {
            Name = name;
            Value = value;
            Id = id;
            Component = component;
        }
    }

    public enum ActiveComponent
    {
        Waveform,
        Frequency
    }
}
