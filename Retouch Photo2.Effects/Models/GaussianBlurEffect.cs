using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s GaussianBlurEffect .
    /// </summary>
    public class GaussianBlurEffect : IEffect
    {
        GaussianBlurPage GaussianBlurPage { get; } = new GaussianBlurPage();

        public EffectType Type => EffectType.GaussianBlur;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new GaussianBlurControl());
        public Page Page => this.GaussianBlurPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.GaussianBlur_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.GaussianBlur_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.GaussianBlur_BlurAmount = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.GaussianBlurPage.BlurAmountSlider.Value = effectManager.GaussianBlur_BlurAmount;
        }
    }
}