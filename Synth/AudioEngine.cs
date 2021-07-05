using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Media.TextFormatting;

namespace Synth
{
	class AudioEngine : WaveProvider32
	{
		#region Public Properties

		public ConcurrentQueue<int> KeyPress;
		public ConcurrentQueue<int> KeyRelease;
		public VoiceManager VoiceManager;
		public List<OscillatorProfile> OscillatorProfiles;
		public List<EnvelopeProfile> OscillatorEnvelopeProfiles;
		public EnvelopeProfile MasterEnvelopeProfile;
		public EnvelopeProfile FilterEnvelopeProfile;
		public FilterProfile FilterProfile;
		public FMPanelProfile FMPanelProfile;
		public DelayProfile DelayProfile;
		public FlangerProfile FlangerProfile;

		#endregion

		#region Constructors

		public AudioEngine() : base(Constants.PlaybackSamplingRate, Constants.Channels)
		{
			KeyPress = new ConcurrentQueue<int>();
			KeyRelease = new ConcurrentQueue<int>();

			OscillatorProfiles = new List<OscillatorProfile>
			{
				new OscillatorProfile(),
				new OscillatorProfile(),
				new OscillatorProfile()
			};

			OscillatorEnvelopeProfiles = new List<EnvelopeProfile>
			{
				new EnvelopeProfile(),
				new EnvelopeProfile(),
				new EnvelopeProfile()
			};

			MasterEnvelopeProfile = new EnvelopeProfile(0.1, 0, 1, 0.1);
			FilterEnvelopeProfile = new EnvelopeProfile(0, 0, 0, 0);
			FilterProfile = new FilterProfile(FilterType.Off, 20000, 0.707, 0, 0, 1);
			FMPanelProfile = new FMPanelProfile();
			DelayProfile = new DelayProfile(0, 2000, 0, 0, 0.5f);
			FlangerProfile = new FlangerProfile();
		}

		#endregion

		#region Public Methods

		public void Init()
        {
			VoiceManager = new VoiceManager(OscillatorProfiles, OscillatorEnvelopeProfiles, MasterEnvelopeProfile, FilterEnvelopeProfile, FilterProfile, FMPanelProfile, DelayProfile, FlangerProfile);
		}

		public void KeyDown(int key)
		{
			lock (KeyPress)
			{
				KeyPress.Enqueue(key);
			}
		}

		public void KeyUp(int key)
		{
				KeyRelease.Enqueue(key);
		}

		#endregion

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int Key;

			for (int i = 0; i < 10 && KeyPress.TryDequeue(out Key); ++i)
			{
				VoiceManager.AddVoice(Key);
			}

			for (int i = 0; i < 10 && KeyRelease.TryDequeue(out Key); ++i)
			{
				VoiceManager.RemoveVoice(Key);
			}

			VoiceManager.Read(buffer, offset, sampleCount);

			return sampleCount;
		}
	}
}
