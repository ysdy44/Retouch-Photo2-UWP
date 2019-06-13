using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s StraightenEffect .
    /// </summary>
    public class StraightenEffect : Effect
    {
        //Icon
        readonly StraightenControl StraightenControl = new StraightenControl();
        //Page
        readonly StraightenPage StraightenPage = new StraightenPage();

        //@Construct
        public StraightenEffect()
        {
            base.Type = EffectType.Straighten;
            base.Button = new Retouch_Photo2.Effects.Button(this.StraightenControl);
            base.Page = this.StraightenPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.Straighten_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Straighten_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.Straighten_Angle = 0;
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.StraightenPage.AnglePicker.Radians = effectManager.Straighten_Angle* 4.0f; ;
        }
    }
}