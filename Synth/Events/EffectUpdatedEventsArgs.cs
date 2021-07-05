using System;

namespace Synth
{
    public class EffectUpdatedEventsArgs : EventArgs
    {
        public EffectType EffectType { get; set; }

        public EffectParameter EffectParameter { get; set; }

        public EffectUpdatedEventsArgs(EffectType effectType = EffectType.Undefined, EffectParameter effectParameter = EffectParameter.Undefined)
        {
            EffectType = effectType;
            EffectParameter = effectParameter;
        }
    }

    public enum EffectType
    {
        Undefined,
        Flanger
    }

    public enum EffectParameter
    {
        Undefined,
        SignalType,
        Frequency,
        Feedback
    }
}
