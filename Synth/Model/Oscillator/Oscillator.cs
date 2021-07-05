using NAudio.Wave;
using System;

namespace Synth
{
	public class Oscillator : IModule
	{
		#region Public Properties

		public int ID { get; set; }

		public Func<float, float, float> SignalGenerator;

		public Envelope Envelope;

		public int Key;


		public OscillatorProfile Profile
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

        public Oscillator(OscillatorProfile profile, EnvelopeProfile envelopeProfile)
		{
			Envelope = new Envelope(envelopeProfile);

			SignalGenerator = SignalGeneratorFactory.GetSignalGenerator(profile.Type);

			phase = new double[Constants.Channels];
			phaseOffset = new double[Constants.Channels];
			frequency = new double[Constants.Channels];
			Profile = profile;
		}

		#endregion

		#region Public Methods

		public void Reset()
		{
			for (int i = 0; i < phase.Length; ++i)
			{
				phase[i] = 0;
			}

			Envelope.Reset();
		}

		public void Press(int key)
		{
			Key = key;

			frequency[0] = profile.Multiplier * 440.0 * Math.Pow(2, (Key - 49.0 + Profile.Coarse + (Profile.Fine / 100.0)) / 12) + profile.Offset;
			frequency[1] = profile.Multiplier * 440.0 * Math.Pow(2, (Key - 49.0 + Profile.Coarse + ((Profile.Fine + Profile.Detune) / 100.0)) / 12) + profile.Offset;

			phaseOffset[1] = profile.PhaseOffset;

			Envelope.Press();
		}

		public void Release()
        {
			Envelope.Release();
        }

		#endregion

		#region Private Members

		private OscillatorProfile profile;

		private double[] phase;
		private double[] phaseOffset;
		public double[] frequency;

        #endregion

        #region Event Handlers

        private void OnProfileChanged(object sender, OscillatorUpdatedEventArgs e)
		{
			switch (e.Component)
            {
				case OscillatorUpdatedComponent.Frequency:
					frequency[0] = profile.Multiplier * 440.0 * Math.Pow(2, (Key - 49.0 + Profile.Coarse + (Profile.Fine / 100.0)) / 12) + profile.Offset;
					frequency[1] = profile.Multiplier * 440.0 * Math.Pow(2, (Key - 49.0 + Profile.Coarse + ((Profile.Fine + Profile.Detune) / 100.0)) / 12) + profile.Offset;
                    break;
				case OscillatorUpdatedComponent.Signal:
					SignalGenerator = SignalGeneratorFactory.GetSignalGenerator(profile.Type);
					break;
			}
		}

        #endregion

        #region Method Overrides

		public double Sample(int channel, double frequencyOffset)
        {
			//phase[channel] = (phase[channel] + ((frequency[channel] + frequencyOffset) * Constants.SamplingInterval)) % 1.0;
			phase[channel] = (phase[channel] + frequency[channel] * Constants.SamplingInterval) % 1.0;
			return SignalTransformation(SignalGenerator((float)(PhaseTransformation(phase[channel]) + phaseOffset[channel]) % 1, (float)frequencyOffset));
		}

        #endregion

        #region Private Methods

		private double PhaseTransformation(double phase)
        {
			double temp = 2 * ((phase + 0.5) % 1) - 1;
			return 0.5 * Math.Sign(temp) * Math.Pow(Math.Abs(temp), profile.Skew) + 1;
		}

		private double SignalTransformation(double signal)
        {
			return Math.Sign(signal) * Math.Pow(Math.Abs(signal), profile.Tension);
		}

        public void Press()
        {
			Envelope.Press();
        }

        #endregion
    }
}
