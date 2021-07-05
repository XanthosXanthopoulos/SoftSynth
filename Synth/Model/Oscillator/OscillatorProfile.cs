using System;
using System.Windows.Forms.VisualStyles;

namespace Synth
{
	public class OscillatorProfile
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

		public double PhaseOffset
        {
			get => phaseOffset;
			set
            {
				if (phaseOffset == value) return;

				phaseOffset = value;
            }
        }

		public int Coarse
        {
			get => coarse;
			set
            {
				if (coarse == value) return;

				coarse = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
            }
        }

		public int Fine
        {
			get => fine;
			set
            {
				if (fine == value) return;

				fine = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
            }
        }

		public int Detune
        {
			get => detune;
			set
            {
				if (detune == value) return;

				detune = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
            }
        }

		public double Tension
        {
			get => tension;
			set
            {
				if (tension == value) return;

				tension = value;
            }
        }

		public double Skew
		{
			get => skew;
			set
			{
				if (skew == value) return;

				skew = value;
			}
		}

		public double Multiplier
        {
			get => multiplier;
			set
            {
				if (multiplier == value) return;

				multiplier = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
			}
		}

		public double Offset
		{
			get => offset;
			set
			{
				if (offset == value) return;

				offset = value;
				OnOscillatorUpdated(OscillatorUpdatedComponent.Frequency);
			}
		}

		public EnvelopeProfile EnvelopeProfile { get; set; }

		#endregion

		#region Private Members

		private SignalType type;

		private double phaseOffset;

		private int coarse;
		private int fine;
		private int detune;

		private double tension;
		private double skew;

		private double multiplier;
		private double offset;

		#endregion

		#region Constructors

		public OscillatorProfile(SignalType type, double phaseOffset, int coarse, int fine, int detune, double skew, double tension, double multiplier, double offset)
        {
			Type = type;
			PhaseOffset = phaseOffset;
			Coarse = coarse;
			Fine = fine;
			Detune = detune;
			Skew = skew;
			Tension = tension;
			Multiplier = multiplier;
			Offset = offset;
        }

        public OscillatorProfile() : this(SignalType.Sine, 0, 24, 0, 0, 1, 1, 1, 0) { }

        #endregion
    }
}
