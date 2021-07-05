using System;

namespace Synth
{
    public class DelayProfile
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

        #endregion

        #region Public Properties

        public double Delay
        {
            get => delay;
            set
            {
                if (delay == value) return;

                OnEffectUpdated();
                delay = value;
            }
        }

        public double MaxDelay
        {
            get => maxDelay;
            set
            {
                if (maxDelay == value) return;

                maxDelay = value;
            }
        }

        public double StereoOffset
        {
            get => stereoOffset;
            set
            {
                if (stereoOffset == value) return;
                OnEffectUpdated();
                stereoOffset = value;
            }
        }

        public double Feedback
        {
            get => feedback;
            set
            {
                if (feedback == value) return;

                feedback = value;
            }
        }

        public double WetDryMix
        {
            get => wetDryMix;
            set
            {
                if (wetDryMix == value) return;

                wetDryMix = value;
            }
        }

        #endregion

        #region Private Members

        private double delay;
        private double maxDelay;
        private double stereoOffset;
        private double feedback;
        private double wetDryMix;

        #endregion

        #region Constructors

        public DelayProfile(double delay, double maxDelay, double feedback, double stereoOffset, double wetDryMix)
        {
            Delay = delay;
            MaxDelay = maxDelay;
            Feedback = feedback;
            StereoOffset = stereoOffset;
            WetDryMix = wetDryMix;
        }

        public DelayProfile() : this(0, 2000, 0, 0, 1)
        {

        }

        #endregion
    }
}
