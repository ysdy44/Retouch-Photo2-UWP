using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.Effects
{
    public class EffectManager
    {
        public GaussianBlurEffectItem GaussianBlurEffectItem = new GaussianBlurEffectItem();
        public DirectionalBlurEffectItem DirectionalBlurEffectItem = new DirectionalBlurEffectItem();
        public OuterShadowEffectItem OuterShadowEffectItem = new OuterShadowEffectItem();

        public OutlineEffectItem OutlineEffectItem = new OutlineEffectItem();

        public EmbossEffectItem EmbossEffectItem = new EmbossEffectItem();
        public StraightenEffectItem StraightenEffectItem = new StraightenEffectItem();


        public ICanvasImage Render(ICanvasImage image)
        {
            if (this.GaussianBlurEffectItem.IsOn) image = this.GaussianBlurEffectItem.Render(image);
            if (this.DirectionalBlurEffectItem.IsOn) image = this.DirectionalBlurEffectItem.Render(image);
            if (this.OuterShadowEffectItem.IsOn) image = this.OuterShadowEffectItem.Render(image);

            if (this.OutlineEffectItem.IsOn) image = this.OutlineEffectItem.Render(image);

            if (this.EmbossEffectItem.IsOn) image = this.EmbossEffectItem.Render(image);
            if (this.StraightenEffectItem.IsOn) image = this.StraightenEffectItem.Render(image);

            return image;
        }
    }
}
