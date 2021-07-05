using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Synth
{
	public class OscillatorViewModel : BaseViewModel
	{
		#region Private Members

		private OscillatorProfile profile;
		private EnvelopeProfile envelopeProfile;

		private int id = -1;

		private SignalType type;

		private int phaseOffset;
		private int detune;
		private int coarse;
		private int fine;
		private int tension;
		private int skew;

		private int multiplier;
		private int offset;

		#endregion

		#region Public Properties

		public int ID
		{
			get
			{
				return id;
			}

			set
			{
				if (id == value) return;

				int oldID = id;

				id = value;
				OnPropertyChanged(nameof(ID));

				SetOscillator(oldID);
			}
		}

		public string Name
		{
			get => "Oscillator " + ToRoman(ID + 1);
		}

		public bool IsMixDisabled
        {
			get => ID != 0;
        }

		public SignalType Type
		{
			get
			{
				return type;
			}

			set
			{
				if (type != value)
                {
					type = value;
					OnPropertyChanged(nameof(Type));
					profile.Type = Type;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Waveform", 0, ID, ActiveComponent.Waveform);
			}
		}

		public int PhaseOffset
		{
			get
			{
				return phaseOffset;
			}

			set
			{
				if (phaseOffset != value)
				{
					phaseOffset = value;
					OnPropertyChanged(nameof(PhaseOffset));
					profile.PhaseOffset = PhaseOffset / 100.0;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Phase Offset", profile.PhaseOffset, ID);
			}
		}

		public int Detune
		{
			get
			{
				return detune;
			}

			set
			{
				if (detune != value)
				{
					detune = value;
					OnPropertyChanged(nameof(Detune));
					profile.Detune = Detune;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Detune", profile.Detune, ID);
			}
		}

		public int Coarse
		{
			get
			{
				return coarse;
			}

			set
			{
				if (coarse != value)
                {
					coarse = value;
					OnPropertyChanged(nameof(Coarse));
					profile.Coarse = Coarse;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Coarse", profile.Coarse, ID);
			}
		}

		public int Fine
		{
			get
			{
				return fine;
			}

			set
			{
				if (fine != value)
				{
					fine = value;
					OnPropertyChanged(nameof(Fine));
					profile.Fine = Fine;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Fine", profile.Fine, ID);
			}
		}

		public int Tension
        {
			get => tension;
			set
            {
				if (tension != value)
                {
					tension = value;
					OnPropertyChanged(nameof(Tension));
					profile.Tension = tension / 100.0;
                }

				OnActiveChanged("Osc_" + (ID + 1) + " Tension", profile.Tension, ID, ActiveComponent.Waveform);
			}
        }

		public int Skew
		{
			get => skew;
			set
			{
				if (skew != value)
				{
					skew = value;
					OnPropertyChanged(nameof(Skew));
					profile.Skew = skew / 100.0;
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Skew", profile.Skew, ID, ActiveComponent.Waveform);
			}
		}

		public int Multiplier
        {
			get => multiplier;
			set
            {
				if (multiplier != value)
                {
					multiplier = value;
					OnPropertyChanged(nameof(Multiplier));
					profile.Multiplier = multiplier / 10000.0;
                }
            }
        }

		public int Offset
		{
			get => offset;
			set
			{
				if (offset != value)
				{
					offset = value;
					OnPropertyChanged(nameof(Offset));
					profile.Offset = offset / 10000.0;
				}
			}
		}

		public int Attack
        {
			get => (int)(envelopeProfile.Attack * 100);
			set
            {
				double temp = value / 100.0;

				if (envelopeProfile.Attack != temp)
				{
					envelopeProfile.Attack = temp;
					OnPropertyChanged(nameof(Attack));
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Attack", envelopeProfile.Attack, ID);
			}
        }

		public int Decay
		{
			get => (int)(envelopeProfile.Decay * 100);
			set
			{
				double temp = value / 100.0;

				if (envelopeProfile.Decay != temp)
				{
					envelopeProfile.Decay = temp;
					OnPropertyChanged(nameof(Decay));
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Decay", envelopeProfile.Decay, ID);
			}
		}

		public int Sustain
		{
			get => (int)(envelopeProfile.Sustain * 100);
			set
			{
				double temp = value / 100.0;

				if (envelopeProfile.Sustain != temp)
				{
					envelopeProfile.Sustain = temp;
					OnPropertyChanged(nameof(Sustain));
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Sustain", envelopeProfile.Sustain, ID);
			}
		}

		public int Release
		{
			get => (int)(envelopeProfile.Release * 100);
			set
			{
				double temp = value / 100.0;

				if (envelopeProfile.Release != temp)
				{
					envelopeProfile.Release = temp;
					OnPropertyChanged(nameof(Release));
				}

				OnActiveChanged("Osc_" + (ID + 1) + " Release", envelopeProfile.Release, ID);
			}
		}

		#endregion

		#region Constructors

		public OscillatorViewModel()
		{

		}

		#endregion

		public double[] GetWaveform()
        {
			double[] waveform = new double[400];

			var func = SignalGeneratorFactory.GetSignalGenerator(Type);
			for (int i = 0; i < 400; ++i)
            {
				double y = func((float)f(i / 399.0), 0);
				waveform[i] = Math.Sign(y) * Math.Pow(Math.Abs(y), profile.Tension);
			}

			return waveform;
        }

		private double f(double x)
        {
			double temp = 2 * ((x + 0.5) % 1) - 1;
			return (0.5 * Math.Sign(temp) * Math.Pow(Math.Abs(temp), profile.Skew) + 1) % 1;
        }

		#region Private Methods

		private void SetOscillator(int oldID)
		{
			AudioEngine engine = IoCContainer.Get<AudioEngine>();

			if (engine.OscillatorProfiles.Count < ID)
			{
				Console.Error.WriteLine("Error: ID in profile out of range. Total oscillators " + engine.OscillatorProfiles.Count + ", requested " + ID + ".");
				ID = oldID;
				return;
			}

			if (profile != null && profile.Equals(engine.OscillatorProfiles[ID])) return;

			envelopeProfile = engine.OscillatorEnvelopeProfiles[ID];
			profile = engine.OscillatorProfiles[ID];

			Type = SignalType.Sine;

			PhaseOffset = (int)(profile.PhaseOffset * 100);
			Detune = profile.Detune;
			Coarse = profile.Coarse;
			Fine = profile.Fine;
			Skew = (int)(profile.Skew * 100);
			Tension = (int)(profile.Tension * 100);
			Multiplier = (int)(profile.Multiplier * 10000);
			Offset = (int)(profile.Offset * 10000);
		}

		#endregion



		private string ToRoman(int number)
		{
			if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
			if (number < 1) return string.Empty;
			if (number >= 1000) return "M" + ToRoman(number - 1000);
			if (number >= 900) return "CM" + ToRoman(number - 900);
			if (number >= 500) return "D" + ToRoman(number - 500);
			if (number >= 400) return "CD" + ToRoman(number - 400);
			if (number >= 100) return "C" + ToRoman(number - 100);
			if (number >= 90) return "XC" + ToRoman(number - 90);
			if (number >= 50) return "L" + ToRoman(number - 50);
			if (number >= 40) return "XL" + ToRoman(number - 40);
			if (number >= 10) return "X" + ToRoman(number - 10);
			if (number >= 9) return "IX" + ToRoman(number - 9);
			if (number >= 5) return "V" + ToRoman(number - 5);
			if (number >= 4) return "IV" + ToRoman(number - 4);
			if (number >= 1) return "I" + ToRoman(number - 1);
			throw new ArgumentOutOfRangeException("something bad happened");
		}
	}
}
