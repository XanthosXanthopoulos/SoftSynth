using System;

namespace Synth
{
    public class LFOOscillatorProfile
    {
		#region Public Events

		/// <summary>
		/// The event that is fired when any child property changes its value 
		/// </summary>
		public event EventHandler<OscillatorUpdatedEventArgs> OscillatorUpdated;

		/// <summary>
		/// Call this to fire a <see cref="PropertyChanged"/> event
		/// </summary>
		/// <param name="name"></param>
		public void OnOscillatorUpdated(OscillatorUpdatedComponent component)
		{
			OscillatorUpdated?.Invoke(this, new OscillatorUpdatedEventArgs(component));
		}

		#endregion

		#region Public Properties

		public SignalType Type
		{
			get => type;
			set
			{
				if (type == value) return;

				type = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Signal);
			}
		}

		public double Frequency
		{
			get => frequency;
			set
			{
				if (frequency == value) return;

				frequency = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
			}
		}

		#endregion

		#region Private Members

		private SignalType type;

		private double frequency;

		#endregion

		#region Constructors

		public LFOOscillatorProfile(SignalType type, double frequency)
		{
			Type = type;
			Frequency = frequency;
		}

		public LFOOscillatorProfile() : this(SignalType.Sine, 1) { }

		#endregion
	}
}
