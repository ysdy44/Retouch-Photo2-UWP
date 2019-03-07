using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Models;
using Retouch_Photo.Effects.Items;
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

        // @static
        public static ICanvasImage Render(EffectManager manager, ICanvasImage image)
        {
            if (manager.GaussianBlurEffectItem.IsOn) image = manager.GaussianBlurEffectItem.Render(image);
            if (manager.DirectionalBlurEffectItem.IsOn) image = manager.DirectionalBlurEffectItem.Render(image);
            if (manager.OuterShadowEffectItem.IsOn) image = manager.OuterShadowEffectItem.Render(image);

            if (manager.OutlineEffectItem.IsOn) image = manager.OutlineEffectItem.Render(image);

            if (manager.EmbossEffectItem.IsOn) image = manager.EmbossEffectItem.Render(image);
            if (manager.StraightenEffectItem.IsOn) image = manager.StraightenEffectItem.Render(image);

            return image;
        }
    }
}
