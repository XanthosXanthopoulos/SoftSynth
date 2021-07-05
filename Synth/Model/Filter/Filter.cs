using System;

namespace Synth
{
	public class Filter : IModule
	{
		#region Public Properties

		public int ID;
		public Envelope Envelope;

		public FilterProfile Profile
        {
			get => profile;
			set
            {
				if (profile == value) return;
				if (profile != null) profile.FilterUpdated -= OnProfileChanged;

				profile = value;
				profile.FilterUpdated += OnProfileChanged;
			}
        }

        private void OnProfileChanged(object sender, FilterUpdatedEventArgs e)
        {
			//Reset();

			UpdateCoefficients();
        }

        #endregion

        #region Constructors

        public Filter(FilterProfile filterProfile, EnvelopeProfile envelopeProfile)
		{
			Profile = filterProfile;

			Envelope = new Envelope(envelopeProfile);

			aCoef = new double[3];
			bCoef = new double[2];

			inputBuffer = new double[2, Constants.Channels];
			outputBuffer = new double[2, Constants.Channels];
		}

		#endregion

		#region Public Methods

		public void Reset()
		{
			for (int i = 0; i < inputBuffer.GetLength(0); ++i)
			{
				for (int j = 0; j < inputBuffer.GetLength(1); ++j)
				{
					inputBuffer[i, j] = 0;
					outputBuffer[i, j] = 0;
				}
			}

			Envelope.Reset();
		}

		public float Apply(float sample, int channel)
		{
			if (profile.Type == FilterType.Off) return sample;

			if (channel == 0)
			{
				envelopeSample = Envelope.Sample();
				UpdateCoefficients();
			}

            double output = aCoef[0] * sample * profile.Overdrive + aCoef[1] * inputBuffer[0, channel] + aCoef[2] * inputBuffer[1, channel] - bCoef[0] * outputBuffer[0, channel] - bCoef[1] * outputBuffer[1, channel];

            inputBuffer[1, channel] = inputBuffer[0, channel];
            inputBuffer[0, channel] = sample * profile.Overdrive;

            outputBuffer[1, channel] = outputBuffer[0, channel];
            outputBuffer[0, channel] = output;

            return (float)(profile.Amount * output + (1 - profile.Amount) * sample);
		}

        #endregion

        #region Private Members

		private FilterProfile profile;

		private double[] aCoef;
		private double[] bCoef;

		private double[,] inputBuffer;
		private double[,] outputBuffer;

		private double envelopeSample;
		private double effectiveCutoff;

        #endregion

        #region Private Methods

		private void UpdateCoefficients()
        {
			effectiveCutoff = Math.Min(profile.CutoffFrequency + profile.ModulationAmount * envelopeSample * envelopeSample * 20000, 20000);

			switch (profile.Type)
			{
				case FilterType.LowPass:
					LPFCoefficients();
					break;
				case FilterType.BandPass:
					BPFCoefficients();
					break;
				case FilterType.HighPass:
					HPFCoefficients();
					break;
			}
		}

		private void LPFCoefficients()
        {
			double w0 = 2 * Math.PI * effectiveCutoff / Constants.SamplingRate;
			double cosw0 = Math.Cos(w0);
			double alpha = Math.Sin(w0) / (2 * profile.Resonance);
			double a0 = 1 + alpha;

			aCoef[0] = (1 - cosw0) / 2 / a0;
			aCoef[1] = (1 - cosw0) / a0;
			aCoef[2] = (1 - cosw0) / 2 / a0;
			bCoef[0] = -2 * cosw0 / a0;
			bCoef[1] = (1 - alpha) / a0;
		}

		private void HPFCoefficients()
		{
			double w0 = 2 * Math.PI * effectiveCutoff / Constants.SamplingRate;
			double cosw0 = Math.Cos(w0);
			double alpha = Math.Sin(w0) / (2 * profile.Resonance);
			double a0 = 1 + alpha;

			aCoef[0] = aCoef[2] = (1 + cosw0) / 2 / a0;
			aCoef[1] = -(1 + cosw0) / a0;
			bCoef[0] = -2 * cosw0 / a0;
			bCoef[1] = 1 - alpha / a0;
		}

		private void BPFCoefficients()
		{
			double w0 = 2 * Math.PI * effectiveCutoff / Constants.SamplingRate;
			double cosw0 = Math.Cos(w0);
			double sinw0 = Math.Sin(w0);
			double alpha = sinw0 / (2 * profile.Resonance);
			double a0 = 1 + alpha;

			aCoef[0] = sinw0 / 2 / a0;
			aCoef[1] = 0;
			aCoef[2] = -sinw0 / 2 / a0;
			bCoef[0] = -2 * cosw0 / a0;
			bCoef[1] = (1 - alpha) / a0;
		}

        public void Press(int key)
        {
			Reset();
			Envelope.Press();
        }

        public void Press()
        {
			Envelope.Press();
		}

        public void Release()
        {
			Envelope.Release();
		}

        #endregion
    }

    public enum FilterType
	{
		Off,
		LowPass,
		BandPass,
		HighPass
	}
}
