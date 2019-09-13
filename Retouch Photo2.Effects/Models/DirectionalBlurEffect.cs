using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's DirectionalBlurEffect .
    /// </summary>
    public class DirectionalBlurEffect : IEffect
    {
        public EffectType Type => EffectType.DirectionalBlur;
        public IEffectPage Page { get; } = new DirectionalBlurPage();
        public IEffectButton Button { get; } = new DirectionalBlurButton();
    }
}