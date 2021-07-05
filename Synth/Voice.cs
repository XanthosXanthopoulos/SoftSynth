using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synth
{
	/// <summary>
	/// A class handling the audio generation for a single key press.
	/// </summary>
	public class Voice : WaveProvider32
	{
		#region Public Members

		/// <summary>
		/// The voice oscillators.
		/// </summary>
		public Oscillator[] Oscillators;

		/// <summary>
		/// The voice master envelope.
		/// </summary>
		public Envelope Envelope;

		/// <summary>
		/// The voice filter.
		/// </summary>
		public Filter Filter;

		/// <summary>
		/// The voice FM Matrix.
		/// </summary>
		public FMPanel FMPanel;

		/// <summary>
		/// The voice flanger effect.
		/// </summary>
		public Flanger Flanger;

		/// <summary>
		/// The voice key state.
		/// </summary>
		public KeyState State;

		/// <summary>
		/// The voice key.
		/// </summary>
		public int Key;

		/// <summary>
		/// The voice volume.
		/// </summary>
		public double Volume;

		#endregion

		#region Constructors

		/// <summary>
		/// Construct a voice with the given component profiles.
		/// </summary>
		/// <param name="oscillatorProfiles">The profiles of the oscillators.</param>
		/// <param name="oscillatorEnvelopeProfiles">The profile for the oscillator specific ADSR envelopes.</param>
		/// <param name="envelopeProfile">The profile for the master ADSR envelope.</param>
		/// <param name="filterEnvelopeProfile"></param>
		/// <param name="filterProfile"></param>
		/// <param name="panelProfile"></param>
		/// <param name="flangerProfile"></param>
		public Voice(List<OscillatorProfile> oscillatorProfiles, List<EnvelopeProfile> oscillatorEnvelopeProfiles, EnvelopeProfile envelopeProfile, EnvelopeProfile filterEnvelopeProfile, FilterProfile filterProfile, FMPanelProfile panelProfile, FlangerProfile flangerProfile)
		{
			Oscillators = new Oscillator[oscillatorProfiles.Count];
			Flanger = new Flanger(flangerProfile);

			for (int i = 0; i < Oscillators.Length; ++i)
			{
				Oscillators[i] = new Oscillator(oscillatorProfiles[i], oscillatorEnvelopeProfiles[i]);
			}

			Envelope = new Envelope(envelopeProfile);
			Filter = new Filter(filterProfile, filterEnvelopeProfile);
			FMPanel = new FMPanel(Oscillators, panelProfile);
			Volume = 0.3;

			State = KeyState.Inactive;
		}

        #endregion

        #region Public Methods

        public void Reset()
		{
			key = -1;

			foreach (Oscillator oscillator in Oscillators)
			{
				oscillator.Reset();
			}

			Envelope.Reset();
			Filter.Reset();

			State = KeyState.Inactive;
		}

		public void PressKey(int key)
		{
			if (this.key != key)
			{
				this.key = key;

				FMPanel.Press(key);
				Filter.Press(key);
			}
			else
            {
				FMPanel.Press();
				Filter.Press();
			}

			State = KeyState.Pressed;

			Envelope.Press();
		}

		public void ReleaseKey()
		{
			State = KeyState.Released;

			FMPanel.Release();

			Envelope.Release();
			Filter.Release();
		}

		#endregion

		#region Private Members

		private int key;

		#endregion

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			FMPanel.Read(buffer, offset, sampleCount);

			float envelopeSample;

            for (int i = 0; i < sampleCount; i += Constants.Channels)
            {
				envelopeSample = Envelope.Sample();

				if (envelopeSample == 0)
				{
					State = KeyState.Inactive;
					key = -1;
					Flanger.Reset();
				}

				for (int channel = 0; channel < Constants.Channels; ++channel)
                {
                    buffer[offset + i + channel] = Flanger.Process(Filter.Apply(buffer[offset + i + channel], channel), channel) * envelopeSample * 0.3f;
                }
            }

            return sampleCount;
		}
	}

	public enum KeyState
	{
		Pressed,
		Released,
		Inactive
	}

}
