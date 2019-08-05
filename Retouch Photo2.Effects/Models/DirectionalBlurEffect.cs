using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s DirectionalBlurEffect .
    /// </summary>
    public class DirectionalBlurEffect : IEffect
    {
        DirectionalBlurPage DirectionalBlurPage { get; } = new DirectionalBlurPage();

        public EffectType Type => EffectType.DirectionalBlur;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new DirectionalBlurControl());
        public Page Page => this.DirectionalBlurPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.DirectionalBlur_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.DirectionalBlur_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.DirectionalBlur_BlurAmount = 0;
            effectManager.DirectionalBlur_Angle = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.DirectionalBlurPage.BlurAmountSlider.Value = effectManager.DirectionalBlur_BlurAmount;
            this.DirectionalBlurPage.AnglePicker.Radians = effectManager.DirectionalBlur_Angle;
        }
    }
}