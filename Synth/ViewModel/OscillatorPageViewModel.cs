using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace Synth
{
	public class OscillatorPageViewModel : BaseViewModel
	{
		#region Public Properties

		public ObservableCollection<OscillatorViewModel> Oscillators { get; set; }

		public string EnvelopeName
        {
			get => "Master Env";
		}

		public string FilterName
		{
			get => "Master Filter";
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

				OnActiveChanged("Master_Env_Attack", envelopeProfile.Attack, 0);
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

				OnActiveChanged("Master_Env_Decay", envelopeProfile.Decay, 0);
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

				OnActiveChanged("Master_Env_Sustain", envelopeProfile.Sustain, 0);
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

				OnActiveChanged("Master_Env_Release", envelopeProfile.Release, 0);
			}
		}

		public FilterType FilterType
        {
			get => filterProfile.Type;
			set
            {
				if (filterProfile.Type == value) return;

				filterProfile.Type = value;
				OnPropertyChanged(nameof(FilterType));
				filterProfile.OnFilterUpdated();
			}
		}

		public int FilterCutoff
        {
			get => filterProfile.CutoffFrequency;
			set
            {
				if (filterProfile.CutoffFrequency != value)
                {
					filterProfile.CutoffFrequency = value;
					filterProfile.OnFilterUpdated();
					OnPropertyChanged(nameof(FilterCutoff));
				}
				
				OnActiveChanged("Filter_Cutoff", filterProfile.CutoffFrequency, 0);
			}
		}

		public int FilterResonance
        {
			get => (int)(filterProfile.Resonance * 100);
			set
            {
				double temp = value / 100.0;

				if (filterProfile.Resonance != temp)
                {
					filterProfile.Resonance = temp;
					filterProfile.OnFilterUpdated();
					OnPropertyChanged(nameof(FilterResonance));
				}

				OnActiveChanged("Filter_Resonance", filterProfile.Resonance, 0);
			}
		}

		public int FilterAmount
        {
			get => (int)(filterProfile.Amount * 100);
			set
            {
				double temp = value / 100.0;

				if (filterProfile.Amount != temp)
                {
					filterProfile.Amount = temp;
					OnPropertyChanged(nameof(FilterAmount));
				}

				OnActiveChanged("Filter_Amount", filterProfile.Amount, 0);
			}
        }

		public int FilterOverdrive
		{
			get => (int)(filterProfile.Overdrive * 100);
			set
			{
				double temp = value / 100.0;

				if (filterProfile.Overdrive != temp)
				{
					filterProfile.Overdrive = temp;
					OnPropertyChanged(nameof(FilterOverdrive));
				}

				OnActiveChanged("Filter_Overdrive", filterProfile.Overdrive, 0);
			}
		}

		public int FilterModulationAmount
        {
			get => (int)(filterProfile.ModulationAmount * 100);
			set
            {
				double temp = value / 100.0;
          
				if (filterProfile.ModulationAmount != temp)
                {
					filterProfile.ModulationAmount = temp;
					OnPropertyChanged(nameof(FilterModulationAmount));
				}

				OnActiveChanged("Filter_Mod", filterProfile.ModulationAmount, 0);
			}
        }

		public int FilterAttack
		{
			get => (int)(filterEnvelopeProfile.Attack * 100);
			set
			{
				double temp = value / 100.0;

				if (filterEnvelopeProfile.Attack != temp)
                {
					filterEnvelopeProfile.Attack = temp;
					OnPropertyChanged(nameof(FilterAttack));
				}
				
				OnActiveChanged("Filter_Attack", filterEnvelopeProfile.Attack, 0);
			}
		}

		public int FilterDecay
		{
			get => (int)(filterEnvelopeProfile.Decay * 100);
			set
			{
				double temp = value / 100.0;

				if (filterEnvelopeProfile.Decay != temp)
                {
                    filterEnvelopeProfile.Decay = temp;
                    OnPropertyChanged(nameof(FilterDecay));
                }

				OnActiveChanged("Filter_Decay", filterEnvelopeProfile.Decay, 0);
			}
		}

		public int FilterSustain
		{
			get => (int)(filterEnvelopeProfile.Sustain * 100);
			set
			{
				double temp = value / 100.0;

				if (filterEnvelopeProfile.Sustain != temp)
                {
                    filterEnvelopeProfile.Sustain = temp;
                    OnPropertyChanged(nameof(FilterSustain));
                }

				OnActiveChanged("Filter_Sustain", filterEnvelopeProfile.Sustain, 0);
			}
		}

		public int FilterRelease
		{
			get => (int)(filterEnvelopeProfile.Release * 100);
			set
			{
				double temp = value / 100.0;

				if (filterEnvelopeProfile.Release != temp)
                {
                    filterEnvelopeProfile.Release = temp;
                    OnPropertyChanged(nameof(FilterRelease));
                }

				OnActiveChanged("Filter_Release", filterEnvelopeProfile.Release, 0);
			}
		}

		public string CurrentControl
        {
			get => currentControl;
			set
            {
				if (currentControl == value) return;

				currentControl = value;
				OnPropertyChanged(nameof(CurrentControl));
            }
        }

		public double CurrentValue
        {
			get => currentValue;
			set
            {
				if (currentValue == value) return;

				currentValue = value;
				OnPropertyChanged(nameof(CurrentValue));
            }
		}

		public double[] Waveform
		{
			get => waveform;
			set
			{
				if (waveform != value)
				{
					waveform = value;
					OnPropertyChanged(nameof(Waveform));
				}
			}
		}

        #region Delay Properties

        public int Delay
        {
			get => (int)delayProfile.Delay;
			set
            {
				if (delayProfile.Delay != value)
				{
					delayProfile.Delay = value;
					OnPropertyChanged(nameof(Delay));
				}

				OnActiveChanged("Delay", delayProfile.Delay, 0);
			}
		}

		public int Feedback
		{
			get => (int)(delayProfile.Feedback * 100);
			set
			{
				double temp = value / 100.0;

				if (delayProfile.Feedback != temp)
				{
					delayProfile.Feedback = temp;
					OnPropertyChanged(nameof(Feedback));
				}

				OnActiveChanged("Feedback", delayProfile.Feedback, 0);
			}
		}

		public int StereoOffset
		{
			get => (int)(delayProfile.StereoOffset * 100);
			set
			{
				double temp = value / 100.0;

				if (delayProfile.StereoOffset != temp)
				{
					delayProfile.StereoOffset = temp;
					OnPropertyChanged(nameof(StereoOffset));
				}

				OnActiveChanged("Stereo_Offset", delayProfile.StereoOffset, 0);
			}
		}

		public int WetDryMix
		{
			get => (int)(delayProfile.WetDryMix * 100);
			set
			{
				double temp = value / 100.0;

				if (delayProfile.WetDryMix != temp)
				{
					delayProfile.WetDryMix = temp;
					OnPropertyChanged(nameof(WetDryMix));
				}

				OnActiveChanged("Wet_Dry_Mix", delayProfile.WetDryMix, 0);
			}
		}

        #endregion

        #region Flanger Properties

        public int Depth
		{
			get => (int)(flangerProfile.Depth * 100);
			set
			{
				double temp = value / 100.0;

				if (flangerProfile.Depth != temp)
				{
					flangerProfile.Depth = temp;
					OnPropertyChanged(nameof(Depth));
				}

				OnActiveChanged("Depth", flangerProfile.Depth, 0);
			}
		}

		public int Rate
		{
			get => (int)flangerProfile.Rate;
			set
			{
				if (flangerProfile.Rate != value)
				{
					flangerProfile.Rate = value;
					OnPropertyChanged(nameof(Rate));
				}

				OnActiveChanged("Rate", flangerProfile.Rate, 0);
			}
		}

		public int FlangerFeedback
		{
			get => (int)(flangerProfile.Feedback * 100);
			set
			{
				double temp = value / 100.0;

				if (flangerProfile.Feedback != temp)
				{
					flangerProfile.Feedback = temp;
					OnPropertyChanged(nameof(FlangerFeedback));
				}

				OnActiveChanged("Feedback", flangerProfile.Feedback, 0);
			}
		}

		public SignalType SignalType
		{
			get => flangerProfile.Type;
			set
			{
				if (flangerProfile.Type == value) return;

				flangerProfile.Type = value;
				OnPropertyChanged(nameof(SignalType));
			}
		}

        #endregion

        #region FM Matrix Properties

        public int OscOneToOne 
		{ 
			get => modulationIndex[0,0]; 
			set
            {
				if (modulationIndex[0, 0] != value)
				{
					modulationIndex[0, 0] = value;
					panelProfile.ModulationIndex[0, 0] = ModulationIndexFormula(value / 100.0, true);
					OnPropertyChanged(nameof(OscOneToOne));
				}

				OnActiveChanged("Osc1_to_Osc1_Mod", value / 100.0, 0);
			}
		}

		public int OscTwoToOne
		{
			get => modulationIndex[0, 1];
			set
			{
				if (modulationIndex[0, 1] == value) return;

				modulationIndex[0, 1] = value;
				panelProfile.ModulationIndex[0, 1] = ModulationIndexFormula(value / 100.0);
				OnPropertyChanged(nameof(OscTwoToOne));
			}
		}

		public int OscThreeToOne
		{
			get => modulationIndex[0, 2]; 
			set
            {
                if (modulationIndex[0, 2] == value) return;

                modulationIndex[0, 2] = value;
                panelProfile.ModulationIndex[0, 2] = ModulationIndexFormula(value / 100.0);
                OnPropertyChanged(nameof(OscThreeToOne));
            }
		}

		public int OscOneToTwo
		{
			get => modulationIndex[1, 0];
			set
			{
				if (modulationIndex[1, 0] == value) return;

				modulationIndex[1, 0] = value;
				panelProfile.ModulationIndex[1, 0] = ModulationIndexFormula(value / 100.0);
				OnPropertyChanged(nameof(OscOneToTwo));
			}
		}

		public int OscTwoToTwo
		{
			get => modulationIndex[1, 1]; 
			set
            {
                if (modulationIndex[1, 1] == value) return;

                modulationIndex[1, 1] = value;
                panelProfile.ModulationIndex[1, 1] = ModulationIndexFormula(value / 100.0, true);
                OnPropertyChanged(nameof(OscTwoToTwo));
            }
		}

		public int OscThreeToTwo
		{
			get => modulationIndex[1, 2]; 
			set
            {
                if (modulationIndex[1, 2] == value) return;

                modulationIndex[1, 2] = value;
                panelProfile.ModulationIndex[1, 2] = ModulationIndexFormula(value / 100.0);
                OnPropertyChanged(nameof(OscThreeToTwo));
            }
		}

		public int OscOneToThree
		{
			get => modulationIndex[2, 0]; 
			set
            {
                if (modulationIndex[2, 0] == value) return;

                modulationIndex[2, 0] = value;
                panelProfile.ModulationIndex[2, 0] = ModulationIndexFormula(value / 100.0);
                OnPropertyChanged(nameof(OscOneToThree));
            }
		}

		public int OscTwoToThree
		{
			get => modulationIndex[2, 1]; 
			set
            {
                if (modulationIndex[2, 1] == value) return;

                modulationIndex[2, 1] = value;
                panelProfile.ModulationIndex[2, 1] = ModulationIndexFormula(value / 100.0);
                OnPropertyChanged(nameof(OscTwoToThree));
            }
		}

		public int OscThreeToThree
		{
			get => modulationIndex[2, 2]; 
			set
            {
                if (modulationIndex[2, 2] == value) return;

                modulationIndex[2, 2] = value;
                panelProfile.ModulationIndex[2, 2] = ModulationIndexFormula(value / 100.0, true);
                OnPropertyChanged(nameof(OscThreeToThree));
            }
		}

		public int OscOnePan
		{
			get => (int)(panelProfile.ModulationIndex[0, 3] * 200 - 100);
			set
			{
				double temp = (value + 100) / 200.0;

				if (panelProfile.ModulationIndex[0, 3] == temp) return;

				panelProfile.ModulationIndex[0, 3] = temp;
				OnPropertyChanged(nameof(OscOnePan));
			}
		}

		public int OscTwoPan
		{
			get => (int)(panelProfile.ModulationIndex[1, 3] * 200 - 100);
			set
			{
				double temp = (value + 100) / 200.0;

				if (panelProfile.ModulationIndex[1, 3] == temp) return;

				panelProfile.ModulationIndex[1, 3] = temp;
				OnPropertyChanged(nameof(OscTwoPan));
			}
		}

		public int OscThreePan
		{
			get => (int)(panelProfile.ModulationIndex[2, 3] * 200 - 100); set
            {
                double temp = (value + 100) / 200.0;

                if (panelProfile.ModulationIndex[2, 3] == temp) return;

                panelProfile.ModulationIndex[2, 3] = temp;
                OnPropertyChanged(nameof(OscThreePan));
            }
		}

		public int OscOneOut
		{
			get => (int)(panelProfile.ModulationIndex[0, 4] * 100);
			set
			{
				double temp = value / 100.0;

				if (panelProfile.ModulationIndex[0, 4] == temp) return;

				panelProfile.ModulationIndex[0, 4] = temp;
				OnPropertyChanged(nameof(OscOneOut));
			}
		}

		public int OscTwoOut
		{
			get => (int)(panelProfile.ModulationIndex[1, 4] * 100);
			set
			{
				double temp = value / 100.0;

				if (panelProfile.ModulationIndex[1, 4] == temp) return;

				panelProfile.ModulationIndex[1, 4] = temp;
				OnPropertyChanged(nameof(OscTwoOut));
			}
		}

		public int OscThreeOut
		{
			get => (int)(panelProfile.ModulationIndex[2, 4] * 100);
			set
			{
				double temp = value / 100.0;

				if (panelProfile.ModulationIndex[2, 4] == temp) return;

				panelProfile.ModulationIndex[2, 4] = temp;
				OnPropertyChanged(nameof(OscThreeOut));
			}
		}

		#endregion

		#endregion

		#region Private Members

		private EnvelopeProfile envelopeProfile;
		private EnvelopeProfile filterEnvelopeProfile;
		private FilterProfile filterProfile;
		private FMPanelProfile panelProfile;
		private DelayProfile delayProfile;
		private FlangerProfile flangerProfile;

		private string currentControl;
		private double currentValue;

		private double[] waveform;
		private int[,] modulationIndex;

		#endregion

		#region Constructors

		public OscillatorPageViewModel()
		{
			Oscillators = new ObservableCollection<OscillatorViewModel>();

			base.ActiveChanged += ActiveControlChanged;

			for (int i = 0; i < IoCContainer.Get<AudioEngine>().OscillatorProfiles.Count; ++i)
			{
                Oscillators.Add(new OscillatorViewModel
                {
					ID = i
                });

                Oscillators[i].ActiveChanged += ActiveControlChanged;
			}

			modulationIndex = new int[3, 3];

			filterProfile = IoCContainer.Get<AudioEngine>().FilterProfile;
			envelopeProfile = IoCContainer.Get<AudioEngine>().MasterEnvelopeProfile;
			panelProfile = IoCContainer.Get<AudioEngine>().FMPanelProfile;
			filterEnvelopeProfile = IoCContainer.Get<AudioEngine>().FilterEnvelopeProfile;
			delayProfile = IoCContainer.Get<AudioEngine>().DelayProfile;
			flangerProfile = IoCContainer.Get<AudioEngine>().FlangerProfile;
		}

		private double ModulationIndexFormula(double percentage, bool feedback = false)
        {
			int sign = Math.Sign(percentage);

			percentage = Math.Abs(percentage);

			if (feedback)
            {
				return sign * (Math.Exp(Math.Log(2.8) * Math.Pow(percentage, 0.7)) - 1);
            }
			else
            {
				return sign * (Math.Exp(Math.Log(30) * Math.Pow(percentage, 0.7)) - 1);
			}
        }

        private void ActiveControlChanged(object sender, ActiveChangedEventArgs e)
        {
			switch(e.Component)
            {
				case ActiveComponent.Waveform:
					Waveform = Oscillators[e.Id].GetWaveform();
					break;
            }

			CurrentControl = e.Name;
			CurrentValue = e.Value;
        }

        #endregion
    }
}
