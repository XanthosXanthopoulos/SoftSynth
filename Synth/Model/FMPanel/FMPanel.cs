using NAudio.Wave;
using System;

namespace Synth
{
    public class FMPanel : WaveProvider32, IModule
    {
        public FMPanelProfile profile;

        public Oscillator[] Oscillators;
        public double[,] CachedOscillators;
        public float[] EnvelopeSamples;

        #region Constructors

        public FMPanel(Oscillator[] oscillators, FMPanelProfile profile)
        {
            Oscillators = oscillators;

            this.profile = profile;

            CachedOscillators = new double[Oscillators.Length, 2];
            EnvelopeSamples = new float[Oscillators.Length];
        }

        public void Press(int key)
        {
            foreach (Oscillator oscillator in Oscillators)
            {
                oscillator.Press(key);
            }

            for (int i = 0; i < CachedOscillators.GetLength(0); ++i)
            {
                for (int j = 0; j < CachedOscillators.GetLength(1); ++j)
                {
                    CachedOscillators[i, j] = 0;
                }
            }
        }

        public void Press()
        {
            foreach (Oscillator oscillator in Oscillators)
            {
                oscillator.Press();
            }
        }

        public void Release()
        {
            foreach (Oscillator oscillator in Oscillators)
            {
                oscillator.Release();
            }
        }

        #endregion

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            for (int i = 0; i < sampleCount; i += Constants.Channels)
            {
                for (int j = 0; j < Oscillators.Length; ++j)
                {
                    EnvelopeSamples[j] = Oscillators[j].Envelope.Sample();
                }

                for (int channel = 0; channel < Constants.Channels; ++channel)
                {
                    for (int j = 0; j < profile.ModulationIndex.GetLength(0); ++j)
                    {
                        double frequencyOffset = 0;

                        for (int k = 0; k < profile.ModulationIndex.GetLength(0); ++k)
                        {
                            frequencyOffset += profile.ModulationIndex[j, k] * CachedOscillators[k, channel];
                        }

                        CachedOscillators[j, channel] = Oscillators[j].Sample(channel, frequencyOffset) * EnvelopeSamples[j];

                        buffer[offset + i + channel] += (float)(CachedOscillators[j, channel] * profile.ModulationIndex[j, 4] * GetChannelGain(channel, profile.ModulationIndex[j, 3]));
                    }
                }
            }

            return sampleCount;
        }

        private float GetChannelGain(int channel, double balance)
        {
            if (Constants.Channels == 1) return 1;

            if (channel % 2 == 0)
            {
                return (float)Math.Cos(Math.PI * 0.5 * balance);
            }
            else
            {
                return (float)Math.Sin(Math.PI * 0.5 * balance);
            }
        }
    }
}
