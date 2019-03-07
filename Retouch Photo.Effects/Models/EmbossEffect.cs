using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;

namespace Retouch_Photo.Effects.Models
{
    public class EmbossEffect : Effect
    {
        public EmbossPage page = new EmbossPage();

        public EmbossEffect()
        {
            base.Type = EffectType.Emboss;
            base.Icon = new EmbossControl();
            base.Page = this.page;
        }
        
        public override EffectItem GetItem(EffectManager effectManager) => effectManager.EmbossEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.EmbossEffectItem.Amount = 1;
            effectManager.EmbossEffectItem.Angle = 0;
        }
        
    }
}

