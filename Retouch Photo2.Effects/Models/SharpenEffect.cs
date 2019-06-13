using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s SharpenEffect .
    /// </summary>
    public class SharpenEffect : Effect
    {
        //Icon
        readonly SharpenControl SharpenControl = new SharpenControl();
        //Page
        readonly SharpenPage SharpenPage = new SharpenPage();

        //@Construct
        public SharpenEffect()
        {
            base.Type = EffectType.Sharpen;
            base.Button = new Retouch_Photo2.Effects.Button(this.SharpenControl);
            base.Page = this.SharpenPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.Sharpen_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Sharpen_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.Sharpen_Amount = 0;
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.SharpenPage.AmountSlider.Value = effectManager.Sharpen_Amount * 10.0f;
        }
    }
}