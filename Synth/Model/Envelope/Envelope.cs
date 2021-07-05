using System;

namespace Synth
{
	public class Envelope
	{
		#region Constructors

		public Envelope(EnvelopeProfile profile)
		{
			this.profile = profile;
		}

		#endregion

		#region Public Methods

		public void Press()
        {
			State = EnvelopeState.Attack;
		}

		public void Release()
        {
			State = EnvelopeState.Release;
		}

		public void Reset()
		{
			amplitude = 0;
			State = EnvelopeState.Off;
		}

		public float Sample()
		{
			switch (State)
			{
				case EnvelopeState.Attack:
					amplitude = profile.AttackBase + amplitude * profile.AttackFeedback;
                    if (amplitude >= 1)
                    {
						amplitude = 1;
						State = EnvelopeState.Decay;
                    }
					break;
				case EnvelopeState.Decay:
					amplitude = profile.DecayBase + amplitude * profile.DecayFeedback;
					if (amplitude <= profile.Sustain)
					{
						amplitude = profile.Sustain;
						State = EnvelopeState.Sustain;
					}
					break;
				case EnvelopeState.Sustain:
					amplitude = profile.Sustain;
					break;
				case EnvelopeState.Release:
					amplitude = profile.ReleaseBase + amplitude * profile.ReleaseFeedback;
					if (amplitude <= 0)
					{
						amplitude = 0;
						State = EnvelopeState.Off;
					}
					break;
			}

			return (float)amplitude;
		}

		#endregion

		#region Private Members

		private readonly EnvelopeProfile profile;

		private EnvelopeState State;

		private double amplitude;

		#endregion
	}
}
