using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;

namespace Synth
{
    class VoiceManager : WaveProvider32
	{
		#region Public Members

		public IDictionary<int, Voice> Voices;
		public Delay Delay;

		#endregion

		#region Private Members

		public int bufferLength = -1;
		private float[][] buffers;
		private bool[] activeVoices;

		#endregion

		#region Constructors

		public VoiceManager(List<OscillatorProfile> oscillatorProfiles, List<EnvelopeProfile> oscillatorEnvelopeProfiles, EnvelopeProfile envelopeProfile, EnvelopeProfile filterEnvelopeProfile, FilterProfile filterProfile, FMPanelProfile panelProfile, DelayProfile delayProfile, FlangerProfile flangerProfile)
		{
			Voices = new Dictionary<int, Voice>(Constants.MaxVoices);
			buffers = new float[Constants.MaxVoices][];
			activeVoices = new bool[Constants.MaxVoices];
			Delay = new Delay(delayProfile);

			for (int i = 0; i < Constants.MaxVoices; ++i)
			{
				buffers[i] = new float[Constants.Channels * Constants.SamplesPerChannel];

				Voices.Add(i, new Voice(oscillatorProfiles, oscillatorEnvelopeProfiles, envelopeProfile, filterEnvelopeProfile, filterProfile, panelProfile, flangerProfile));
			}
		}

		#endregion

		#region Public Methods

		public void AddVoice(int key)
		{
			Voice voice;

			if (Voices.TryGetValue(key, out voice))
			{
				if (voice.State == KeyState.Inactive)
                {
					voice.Reset();
                }

				voice.PressKey(key);
			}
			else
			{
				foreach (KeyValuePair<int, Voice> pair in Voices)
				{
					if (pair.Value.State == KeyState.Inactive)
					{
						voice = pair.Value;

						voice.Reset();
						voice.PressKey(key);

						Voices.Remove(pair.Key);
						Voices.Add(key, voice);

						break;
					}
				}
			}
		}

		public void RemoveVoice(int key)
		{
			Voice voice;

			if (Voices.TryGetValue(key, out voice))
			{
				if (voice.State != KeyState.Inactive)
				{
					voice.State = KeyState.Released;
					voice.ReleaseKey();
				}
			}
		}

		#endregion

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			for (int i = 0; i < sampleCount; ++i)
			{
				buffer[offset + i] = 0;
			}

            Parallel.ForEach(Voices, (voice, state, index) =>
            {
				if (voice.Value.State != KeyState.Inactive)
				{
					activeVoices[index] = true;

					Array.Clear(buffers[index], offset, sampleCount);

					voice.Value.Read(buffers[index], offset, sampleCount);
				}
				else
                {
					activeVoices[index] = false;
                }
            });

			Vector<float> v0 = Vector<float>.Zero;

			float[] b = new float[buffer.Length / sizeof(float)];

			for (int i = 0; i < Constants.MaxVoices; ++i)
			{ 
				if (activeVoices[i])
				{
					for (int j = 0; j < sampleCount; j += Vector<float>.Count)
					{
						v0 = new Vector<float>(b, offset + j) + new Vector<float>(buffers[i], offset + j);
						v0.CopyTo(b, offset + j);
					}
				}
			}


			Buffer.BlockCopy(b, 0, buffer, 0, buffer.Length);

            for (int i = offset; i < sampleCount; i += Constants.Channels)
            {
                for (int channel = 0; channel < Constants.Channels; ++channel)
                {
                    buffer[i + offset + channel] = Delay.Process(buffer[i + offset + channel], channel);
                }
            }

            return sampleCount;
		}
	}
}
