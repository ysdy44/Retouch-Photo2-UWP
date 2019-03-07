using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;
using Windows.UI;

namespace Retouch_Photo.Effects.Models
{
    public class OuterShadowEffect : Effect
    {
        public OuterShadowPage page = new OuterShadowPage();

        public OuterShadowEffect()
        {
            base.Type = EffectType.OuterShadow;
            base.Icon = new OuterShadowControl();
            base.Page = this.page;
        }

        public override EffectItem GetItem(EffectManager effectManager) => effectManager.OuterShadowEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.OuterShadowEffectItem.Radius = 0;
            effectManager.OuterShadowEffectItem.Opacity = 0.5f;
            effectManager.OuterShadowEffectItem.Color = Colors.Black;
            effectManager.OuterShadowEffectItem.Offset = 0;
            effectManager.OuterShadowEffectItem.Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
    }
}


