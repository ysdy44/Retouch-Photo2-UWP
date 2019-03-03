using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.EffectsControls;
using Retouch_Photo.Pages.EffectPages;
using System;
using Windows.UI;

namespace Retouch_Photo.Models.Effects
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

            effectManager.StraightenEffectItem.Angle  = 0;
        }

    }


    public class StraightenEffectItem : EffectItem
    {
        public float Angle;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.StraightenEffect
            {
                Angle = this.Angle,
                MaintainSize = true,
                Source = image,
            };
        }
    }
}

