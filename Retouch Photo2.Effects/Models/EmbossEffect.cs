using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s EmbossEffect .
    /// </summary>
    public class EmbossEffect : Effect
    {
        //Icon
        readonly EmbossControl EmbossControl = new EmbossControl();
        //Page
        readonly EmbossPage EmbossPage = new EmbossPage();

        //@Construct
        public EmbossEffect()
        {
            base.Type = EffectType.Emboss;
            base.Button = new Retouch_Photo2.Effects.Button(this.EmbossControl);
            base.Page = this.EmbossPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.Emboss_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Emboss_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.Emboss_Amount = 0;
            effectManager.Emboss_Angle = 0;
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.EmbossPage.AmountSlider.Value = effectManager.Emboss_Amount;
            this.EmbossPage.AnglePicker.Radians = effectManager.Emboss_Angle;
        }
    }
}