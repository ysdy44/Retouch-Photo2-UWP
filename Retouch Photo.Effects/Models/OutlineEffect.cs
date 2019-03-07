using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;

namespace Retouch_Photo.Effects.Models
{
    public class OutlineEffect : Effect
    {
        public OutlinePage page = new OutlinePage();

        public OutlineEffect()
        {
            base.Type = EffectType.Outline;
            base.Icon = new OutlineControl();
            base.Page = this.page;
        }

        public override EffectItem GetItem(EffectManager effectManager) => effectManager.OutlineEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.OutlineEffectItem.Size = 1;
        }

    }
}

