using System;

namespace Synth
{
	static class Constants
	{
		public static int SamplingRate = 44100;
		public static int PlaybackSamplingRate = 44100;
		public static double SamplingInterval = 1.0 / SamplingRate;
		public static int Channels = 2;
		public static int Latency = 20;

		public static int MaxVoices = 20;
		public static int SamplesPerChannel;
	}
}
