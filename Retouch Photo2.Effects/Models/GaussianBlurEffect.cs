using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s GaussianBlurEffect .
    /// </summary>
    public class GaussianBlurEffect : Effect
    {
        //Icon
        readonly GaussianBlurControl GaussianBlurControl = new GaussianBlurControl();
        //Page
        readonly GaussianBlurPage GaussianBlurPage = new GaussianBlurPage();

        //@Construct
        public GaussianBlurEffect()
        {
            base.Type = EffectType.GaussianBlur;
            base.Button = new Retouch_Photo2.Effects.Button(this.GaussianBlurControl);
            base.Page = this.GaussianBlurPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.GaussianBlur_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.GaussianBlur_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.GaussianBlur_BlurAmount = 0;
         }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.GaussianBlurPage.BlurAmountSlider.Value = effectManager.GaussianBlur_BlurAmount;
        }
    }
}