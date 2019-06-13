using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s DirectionalBlurEffect .
    /// </summary>
    public class DirectionalBlurEffect : Effect
    {
        //Icon
        readonly DirectionalBlurControl DirectionalBlurControl = new DirectionalBlurControl();
        //Page
        readonly DirectionalBlurPage DirectionalBlurPage = new DirectionalBlurPage();

        //@Construct
        public DirectionalBlurEffect()
        {
            base.Type = EffectType.DirectionalBlur;
            base.Button = new Retouch_Photo2.Effects.Button(this.DirectionalBlurControl);
            base.Page =this.DirectionalBlurPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.DirectionalBlur_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.DirectionalBlur_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.DirectionalBlur_BlurAmount = 0;
            effectManager.DirectionalBlur_Angle = 0;
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.DirectionalBlurPage.BlurAmountSlider.Value = effectManager.DirectionalBlur_BlurAmount;
            this.DirectionalBlurPage.AnglePicker.Radians = effectManager.DirectionalBlur_Angle;
        }
    }
}