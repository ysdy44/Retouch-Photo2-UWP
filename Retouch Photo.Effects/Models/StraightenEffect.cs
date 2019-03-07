using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;

namespace Retouch_Photo.Effects.Models
{
    public class StraightenEffect : Effect
    {
        public StraightenPage page = new StraightenPage();

        public StraightenEffect()
        {
            base.Type = EffectType.Straighten;
            base.Icon = new StraightenControl();
            base.Page = this.page;
        }

        public override EffectItem GetItem(EffectManager effectManager) => effectManager.StraightenEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.StraightenEffectItem.Angle = 0;
        }

    }
}

