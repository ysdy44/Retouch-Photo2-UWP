using Retouch_Photo2.Effects.Buttons;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// IEffect's StraightenEffect .
    /// </summary>
    public class StraightenEffect : IEffect
    {
        public EffectType Type => EffectType.Straighten;
        public IEffectPage Page { get; } = new StraightenPage();
        public IEffectButton Button { get; } = new StraightenButton();
    }
}