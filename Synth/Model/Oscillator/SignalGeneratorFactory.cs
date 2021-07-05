using System;
using System.ComponentModel; // Should be removed and implemented through value converter iterface

namespace Synth
{
	public class SignalGeneratorFactory
	{
		#region Static Methods

		public static Func<float, float, float> GetSignalGenerator(SignalType type)
		{
			switch (type)
			{
				case SignalType.Sine:
					return (x, y) => (float)Math.Sin(2 * Math.PI * x + y);
				case SignalType.Square:
					return (x, y) => Math.Sin(2 * Math.PI * x) > 0 ? 1 : -1;
				case SignalType.Saw:
					return (x, y) =>  2 * x - 1;
				case SignalType.Triangle:
					return (x, y) => (float)(2 * Math.Asin(Math.Sin(2 * Math.PI * x)) / Math.PI);
				case SignalType.Noise:
					Random random = new Random();
					return (x, y) => 2 * (float)random.NextDouble() - 1;
				case SignalType.AnalogSquare:
					return (x, y) =>
					{
						double sum = 0;
						for (int i = 1; i < 50; i += 2) 
						{
							sum += Math.Sin(i * 2 * Math.PI * x) / i;
						}
						return (float)(sum * 4 / Math.PI / 1.18);
					};
				case SignalType.AnalogSaw:
					return (x, y) =>
					{
						double sum = 0;
						int sign = 1;
						for (int i = 1; i < 25; ++i)
						{
							sum += sign * Math.Sin(i * 2 * Math.PI * ((x + 0.5f) % 1)) / i;
							sign = -sign;
						}

						return (float)(2 * sum / Math.PI / 1.14);
					};
				default:
					return (x, y) => 0;
			}
		}

		#endregion
	}

	[TypeConverter(typeof(EnumDescriptionTypeConverter))]
	public enum SignalType
	{
		Sine,
		Square,
		Saw,
		Triangle,
		Noise,
		[Description("Analog Square")]
		AnalogSquare,
		[Description("Analog Saw")]
		AnalogSaw
	}
}
