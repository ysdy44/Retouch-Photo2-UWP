using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's SharpenEffect .
    /// </summary>
    public class SharpenEffect : IEffect
    {
        public EffectType Type => EffectType.Sharpen;
        public IEffectPage Page { get; } = new SharpenPage();
        public IEffectButton Button { get; } = new SharpenButton();
    }
}