using System;

namespace Synth
{
    public class Flanger
    {
        #region Public Properties

        public FlangerProfile Profile
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

        private FlangerProfile profile;

        private LFOOscillator LFO;
        private Delay Delay;

        int MaxDelay;

        #endregion

        #region Event Handlers

        private void OnEffectChanged(object sender, EffectUpdatedEventsArgs e)
        {
            if (e.EffectType != EffectType.Flanger) return;

            switch(e.EffectParameter)
            {
                case EffectParameter.SignalType:
                    LFO.Profile.Type = profile.Type;
                    break;
                case EffectParameter.Frequency:
                    LFO.Profile.Frequency = profile.Rate;
                    break;
                case EffectParameter.Feedback:
                    Delay.Profile.Feedback = profile.Feedback;
                    break;
            }
        }

        #endregion

        #region Constructors

        public Flanger(FlangerProfile profile)
        {
            Profile = profile;
            MaxDelay = 7;

            LFO = new LFOOscillator(new LFOOscillatorProfile(profile.Type, profile.Rate));
            Delay = new Delay(new DelayProfile(Profile.Depth * MaxDelay, MaxDelay + 1, profile.Feedback, 0, 0.5f));
        }

        #endregion

        public void Reset()
        {
            Delay.Reset();
            LFO.Reset();
        }

        public float Process(float sample, int channel)
        {
            Delay.Profile.Delay = (float)((LFO.Sample(channel) * 0.5 + 0.5) * MaxDelay * profile.Depth);
            return Delay.Process(sample, channel);
        }
    }
}
