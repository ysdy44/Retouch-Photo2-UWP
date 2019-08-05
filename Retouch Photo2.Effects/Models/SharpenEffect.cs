using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s SharpenEffect .
    /// </summary>
    public class SharpenEffect : IEffect
    {
        SharpenPage SharpenPage { get; } = new SharpenPage();

        public EffectType Type => EffectType.Sharpen;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new SharpenControl());
        public Page Page => this.SharpenPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.Sharpen_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Sharpen_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.Sharpen_Amount = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.SharpenPage.AmountSlider.Value = effectManager.Sharpen_Amount * 10.0f;
        }
    }
}