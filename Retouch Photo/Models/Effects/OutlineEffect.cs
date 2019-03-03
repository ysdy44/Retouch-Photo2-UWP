using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.EffectsControls;
using Retouch_Photo.Pages.EffectPages;
using System;
using Windows.UI;

namespace Retouch_Photo.Models.Effects
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


    public class OutlineEffectItem : EffectItem
    {
        private int size = 1;
        public int Size
        {
            get => this.size;
            set
            {
                Mode = (value > 0) ? MorphologyEffectMode.Dilate : MorphologyEffectMode.Erode;

                int s = Math.Abs(value);
                Height = Width = s > 90 ? 90 : s;

                this.size = value;
            }
        }

        /// <summary> Dilate and Erode : 扩张与侵蚀</summary>
        MorphologyEffectMode Mode;
        int Width = 1;
        int Height = 1;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.MorphologyEffect
            {
                Source = image,
                Mode = this.Mode,
                Width = this.Width,
                Height = this.Height,
            };
        }
    }
}

