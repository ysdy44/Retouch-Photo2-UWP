using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Pages;

namespace Retouch_Photo.Effects.Models
{
    public class DirectionalBlurEffect : Effect
    {
        public DirectionalBlurPage page = new DirectionalBlurPage();

        public DirectionalBlurEffect()
        {
            base.Type = EffectType.DirectionalBlur;
            base.Icon = new DirectionalBlurControl();
            base.Page = this.page;
        }
        
        public override EffectItem GetItem(EffectManager effectManager) => effectManager.DirectionalBlurEffectItem;
        public override void SetPage(EffectManager effectManager) => this.page.EffectManager = effectManager;
        public override void Reset(EffectManager effectManager)
        {
            this.page.EffectManager = null;

            effectManager.DirectionalBlurEffectItem.BlurAmount = 0;
            effectManager.DirectionalBlurEffectItem.Angle = 0;
        }

    }


    public class DirectionalBlurEffectItem : EffectItem
    {
        public float BlurAmount;
        public float Angle;
        
        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount,
                Angle = -this.Angle,
            };
        }
    }
}

