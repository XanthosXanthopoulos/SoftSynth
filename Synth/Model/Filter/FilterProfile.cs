using System;

namespace Synth
{
    public class FilterProfile
    {
        #region Public Events

        /// <summary>
        /// The event that is fired when any child property changes its value 
        /// </summary>
        public event EventHandler<FilterUpdatedEventArgs> FilterUpdated;

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnFilterUpdated()
        {
            FilterUpdated?.Invoke(this, new FilterUpdatedEventArgs());
        }

        #endregion

        #region Public Properties

        public FilterType Type { get; set; }

        public int CutoffFrequency { get; set; }

        public double Resonance { get; set; }

        public double Amount { get; set; }

        public double ModulationAmount { get; set; }

        public double Overdrive { get; set; }

        #endregion

        #region Constructors

        public FilterProfile(FilterType type, int cutoffFrequency, double resonance, double amount, double modulationAmount, double overdrive)
        {
            Type = type;
            CutoffFrequency = cutoffFrequency;
            Resonance = resonance;
            Amount = amount;
            ModulationAmount = modulationAmount;
            Overdrive = overdrive;
        }

        public FilterProfile() : this(FilterType.Off, 20000, 0.5, 0, 0, 1) { }

        #endregion
    }
}
