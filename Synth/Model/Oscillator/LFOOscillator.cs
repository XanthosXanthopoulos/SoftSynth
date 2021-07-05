using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synth
{
    public class LFOOscillator
    {
		#region Public Properties

		public int ID { get; set; }

		public Func<float, float, float> SignalGenerator;

		public LFOOscillatorProfile Profile
		{
			get => profile;
			set
			{
				if (profile == value) return;
				if (profile != null) profile.OscillatorUpdated -= OnProfileChanged;

				profile = value;
				profile.OscillatorUpdated += OnProfileChanged;
			}
		}

		#endregion

		#region Constructors

		public LFOOscillator(LFOOscillatorProfile profile)
		{
			SignalGenerator = SignalGeneratorFactory.GetSignalGenerator(profile.Type);

			phase = new double[Constants.Channels];
			frequency = new double[Constants.Channels];
			Profile = profile;

			frequency[0] = frequency[1] = profile.Frequency;
		}

		#endregion

		#region Public Methods

		public void Reset()
		{
			for (int i = 0; i < phase.Length; ++i)
			{
				phase[i] = 0;
			}
		}

		#endregion

		#region Private Members

		private LFOOscillatorProfile profile;

		private double[] phase;
		public double[] frequency;

		#endregion

		#region Event Handlers

		private void OnProfileChanged(object sender, OscillatorUpdatedEventArgs e)
		{
			switch (e.Component)
			{
				case OscillatorUpdatedComponent.Frequency:
					frequency[0] = profile.Frequency;
					frequency[1] = profile.Frequency;
					break;
				case OscillatorUpdatedComponent.Signal:
					SignalGenerator = SignalGeneratorFactory.GetSignalGenerator(profile.Type);
					break;
			}
		}

		#endregion

		#region Method Overrides

		public double Sample(int channel)
		{
			phase[channel] = (phase[channel] + (frequency[channel] * Constants.SamplingInterval)) % 1.0;
			return SignalGenerator((float)phase[channel], 0);
		}

		#endregion
	}
}
