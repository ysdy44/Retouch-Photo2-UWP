using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;

namespace Retouch_Photo.Effects.Items
{
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
