using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's GaussianBlurEffect .
    /// </summary>
    public class GaussianBlurEffect : IEffect
    {
        public EffectType Type => EffectType.GaussianBlur;
        public IEffectPage Page { get; } = new GaussianBlurPage();
        public IEffectButton Button { get; } = new GaussianBlurButton();
    }
}