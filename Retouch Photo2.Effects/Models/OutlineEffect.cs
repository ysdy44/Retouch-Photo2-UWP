using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s OutlineEffect .
    /// </summary>
    public class OutlineEffect : Effect
    {
        //Icon
        readonly OutlineControl OutlineControl = new OutlineControl();
        //Page
        readonly OutlinePage OutlinePage = new OutlinePage();

        //@Construct
        public OutlineEffect()
        {
            base.Type = EffectType.Outline;
            base.Button = new Retouch_Photo2.Effects.Button(this.OutlineControl);
            base.Page = this.OutlinePage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.Outline_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Outline_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.Outline_Size = 0;
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.OutlinePage.SizeSlider.Value = effectManager.Outline_Size;
        }
    }
}