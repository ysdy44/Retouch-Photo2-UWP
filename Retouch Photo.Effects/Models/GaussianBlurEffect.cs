using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;

namespace Retouch_Photo.Effects.Models
{
    public class GaussianBlurEffect : Effect
    {
        public GaussianBlurPage page = new GaussianBlurPage();

        public GaussianBlurEffect()
        {
            base.Type = EffectType.GaussianBlur;
            base.Icon = new GaussianBlurControl();
            base.Page = this.page;
        }
        
        public override EffectItem GetItem(EffectManager effectManager) => effectManager.GaussianBlurEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.GaussianBlurEffectItem.BlurAmount = 0;
        }

    }


    public class GaussianBlurEffectItem : EffectItem
    {
        public float BlurAmount;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount
            };
        }
    }
}

