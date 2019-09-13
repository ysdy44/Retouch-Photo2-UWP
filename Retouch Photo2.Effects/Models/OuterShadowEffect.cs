using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's OuterShadowEffect .
    /// </summary>
    public class OuterShadowEffect : IEffect
    {
        public EffectType Type => EffectType.OuterShadow;
        public IEffectPage Page { get; } = new OuterShadowPage();
        public IEffectButton Button { get; } = new OuterShadowButton();
    }
}