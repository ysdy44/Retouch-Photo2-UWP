using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's EmbossEffect .
    /// </summary>
    public class EmbossEffect : IEffect
    {
        public EffectType Type => EffectType.Emboss;
        public IEffectPage Page { get; } = new EmbossPage();
        public IEffectButton Button { get; } = new EmbossButton();
    }
}