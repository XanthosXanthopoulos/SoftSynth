using System;

namespace Synth
{
    public class FlangerProfile
    {
        #region Public Events

        /// <summary>
        /// The event that is fired when any child property changes its value 
        /// </summary>
        public event EventHandler<EffectUpdatedEventsArgs> EffectUpdated;

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnEffectUpdated()
        {
            EffectUpdated?.Invoke(this, new EffectUpdatedEventsArgs());
        }

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnEffectUpdated(EffectUpdatedEventsArgs args)
        {
            EffectUpdated?.Invoke(this, args);
        }

        #endregion

        #region Public Properties

        public SignalType Type
        {
            get => type;
            set
            {
                if (type == value) return;

                type = value;
                OnEffectUpdated(new EffectUpdatedEventsArgs(EffectType.Flanger, EffectParameter.SignalType));
            }
        }

        public double Depth
        {
            get => depth;
            set
            {
                if (depth == value) return;

                depth = value;
            }
        }

        public double Rate
        {
            get => rate;
            set
            {
                if (rate == value) return;

                rate = value;
                OnEffectUpdated(new EffectUpdatedEventsArgs(EffectType.Flanger, EffectParameter.Frequency));
            }
        }

        public double Feedback
        {
            get => feedback;
            set
            {
                if (feedback == value) return;

                feedback = value;
                OnEffectUpdated(new EffectUpdatedEventsArgs(EffectType.Flanger, EffectParameter.Feedback));
            }
        }

        #endregion

        #region Private Members

        private SignalType type;
        private double depth;
        private double rate;
        private double feedback;

        #endregion

        #region Constructors

        public FlangerProfile(float depth, float feedback, float rate, SignalType type)
        {
            Depth = depth;
            Feedback = feedback;
            Rate = rate;
            Type = type;
        }

        public FlangerProfile() : this(0, 0, 10, SignalType.Sine)
        {

        }

        #endregion
    }
}
