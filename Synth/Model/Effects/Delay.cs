using System;
using System.Threading;

namespace Synth
{
    public class Delay
    {
        #region Public Members

        public DelayProfile Profile
        {
            get => profile;
            set
            {
                if (profile == value) return;
                if (profile != null) profile.EffectUpdated -= OnEffectChanged;

                profile = value;
                profile.EffectUpdated += OnEffectChanged;
            }
        }

        #endregion

        #region Private Members

        private double[][] buffer;
        private double[][] offsetBuffer;
        private int[,] Index;

        private double[] fraction;

        private DelayProfile profile;

        #endregion

        #region Event Handlers

        private void OnEffectChanged(object sender, EffectUpdatedEventsArgs e)
        {
            double samples = profile.Delay * Constants.SamplingRate / 1000;
            double offset = profile.Delay * Math.Abs(profile.StereoOffset) * 0.25f * Constants.SamplingRate / 1000;

            int[,] index = new int[Constants.Channels, 5];

            for (int channel = 0; channel < Constants.Channels; ++channel)
            {
                fraction[channel] = samples - (int)samples;

                index[channel, 4] = Index[channel, 4];
                index[channel, 3] = channel % 2 == (profile.StereoOffset > 0 ? 0 : 1) ? Index[channel, 4] : (Index[channel, 4] - (int)offset + offsetBuffer[0].Length) % offsetBuffer[0].Length;
                index[channel, 2] = Index[channel, 2];
                index[channel, 1] = (Index[channel, 2] - (int)samples + buffer[0].Length) % buffer[0].Length;
                index[channel, 0] = (index[channel, 1] - 1 + buffer[0].Length) % buffer[0].Length;
            }

            Interlocked.Exchange(ref Index, index);
        }

        #endregion

        #region Constructors

        public Delay(DelayProfile profile)
        {
            Profile = profile;

            buffer = new double[Constants.Channels][];
            offsetBuffer = new double[Constants.Channels][];
            for (int i = 0; i < Constants.Channels; ++i)
            {
                buffer[i] = new double[(int)Math.Ceiling(Constants.SamplingRate * profile.MaxDelay / 1000.0)];
                offsetBuffer[i] = new double[(int)Math.Ceiling(Constants.SamplingRate * profile.MaxDelay * 0.5 / 1000.0)];
            }

            Index = new int[Constants.Channels, 5];
            fraction = new double[Constants.Channels];
        }

        #endregion

        public void Reset()
        {
            for (int i = 0; i < Constants.Channels; ++i)
            {
                for (int j = 0; j < buffer[0].Length; ++j)
                {
                    buffer[i][j] = 0;
                }
            }
        }

        public float Process(float sample, int channel)
        {
            if (profile.Delay == 0) return sample;

            if (Index[channel, 1] == Index[channel, 2] && profile.Delay < 1)
            {
                buffer[channel][Index[channel, 1]] = sample;
            }

            offsetBuffer[channel][Index[channel, 4]] = sample;

            double output = Interpolate(buffer[channel][Index[channel, 0]], buffer[channel][Index[channel, 1]], fraction[channel]);

            buffer[channel][Index[channel, 2]] = offsetBuffer[channel][Index[channel, 3]] + profile.Feedback * output;

            output = profile.WetDryMix * sample + (1 - profile.WetDryMix) * output;

            Index[channel, 0] = (Index[channel, 0] + 1) % buffer[0].Length;
            Index[channel, 1] = (Index[channel, 1] + 1) % buffer[0].Length;
            Index[channel, 2] = (Index[channel, 2] + 1) % buffer[0].Length;
            Index[channel, 3] = (Index[channel, 3] + 1) % offsetBuffer[0].Length;
            Index[channel, 4] = (Index[channel, 4] + 1) % offsetBuffer[0].Length;

            return (float)output;
        }

        private double Interpolate(double x, double y, double ratio)
        {
            return x * ratio + (1 - ratio) * y;
        }
    }
}
