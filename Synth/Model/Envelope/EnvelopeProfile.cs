using System;

namespace Synth
{
    public class EnvelopeProfile
    {
        #region Public Properties

        public double Attack 
        {
            get => attack; 
            set
            {
                if (attack == value) return;

                attack = value;
                CalculateAttackCoefficients();
            }
        }

        public double Decay 
        { 
            get => decay; 
            set
            {
                if (decay == value) return;

                decay = value;
                CalculateDecayCoefficients();
            }
        }

        public double Sustain 
        {
            get => sustain; 
            set
            {
                if (sustain == value) return;

                sustain = value;
                CalculateDecayCoefficients();
            }
        }

        public double Release 
        { 
            get => release; 
            set
            {
                if (release == value) return;

                release = value;
                CalculateReleaseCoefficients();
            }
        }

        public double AttackRatio 
        { 
            get => attackRatio; 
            set
            {
                if (attackRatio == value) return;

                attackRatio = value;
                CalculateAttackCoefficients();
            }
        }

        public double DecayRatio
        {
            get => decayRatio;
            set
            {
                if (decayRatio == value) return;

                decayRatio = value;
                CalculateDecayCoefficients();
            }
        }

        public double ReleaseRatio 
        { 
            get => releaseRatio;
            set
            {
                if (releaseRatio == value) return;

                releaseRatio = value;
                CalculateReleaseCoefficients();
            }
        }

        public double AttackBase { get; private set; }

        public double AttackFeedback { get; private set; }

        public double DecayBase { get; private set; }

        public double DecayFeedback { get; private set; }

        public double ReleaseBase { get; private set; }

        public double ReleaseFeedback { get; private set; }

        #endregion

        #region Private Members

        private double attack;
        private double decay;
        private double sustain;
        private double release;

        private double attackRatio =  0.003;
        private double decayRatio = 0.003;
        private double releaseRatio = 0.003;

        #endregion

        #region Constructors

        public EnvelopeProfile(double attack, double decay, double sustain, double release)
        {
            Attack = attack;
            Decay = decay;
            Sustain = sustain;
            Release = release;

            CalculateAttackCoefficients();
            CalculateDecayCoefficients();
            CalculateReleaseCoefficients();
        }

        public EnvelopeProfile() : this(0, 0, 1, 0) 
        {
        }

        #endregion

        #region Private Methods

        private double CalculateCoefficient(double stage, double stageRatio)
        {
            return (stage <= 0) ? 0.0 : Math.Exp(-Constants.SamplingInterval * Math.Log((1.0 + stageRatio) / stageRatio) / stage);
        }

        private void CalculateAttackCoefficients()
        {
            AttackFeedback = CalculateCoefficient(Attack, AttackRatio);
            AttackBase = (1.0 + AttackRatio) * (1.0 - AttackFeedback);
        }

        private void CalculateDecayCoefficients()
        {
            DecayFeedback = CalculateCoefficient(Decay, DecayRatio);
            DecayBase = (Sustain - DecayRatio) * (1.0 - DecayFeedback);
        }

        private void CalculateReleaseCoefficients()
        {
            ReleaseFeedback = CalculateCoefficient(Release, ReleaseRatio);
            ReleaseBase = -ReleaseRatio * (1.0 - ReleaseFeedback);
        }

        #endregion
    }

    public enum EnvelopeState
    {
        Attack,
        Decay,
        Sustain,
        Release,
        Off
    }
}
