using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's OutlineEffect .
    /// </summary>
    public class OutlineEffect : IEffect
    {
        public EffectType Type => EffectType.Outline;
        public IEffectPage Page { get; } = new OutlinePage();
        public IEffectButton Button { get; } = new OutlineButton();
    }
}