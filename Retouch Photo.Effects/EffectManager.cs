using Microsoft.Graphics.Canvas;
using Retouch_Photo.Effects.Items;
using System;

namespace Retouch_Photo.Effects
{
    /// <summary> <see cref = "Effect" />'s manager. </summary>
    public class EffectManager
    {
        //@static
        public static Action Invalidate;


        public GaussianBlurEffectItem GaussianBlurEffectItem = new GaussianBlurEffectItem();
        public DirectionalBlurEffectItem DirectionalBlurEffectItem = new DirectionalBlurEffectItem();
        public SharpenEffectItem SharpenEffectItem = new SharpenEffectItem();
        public OuterShadowEffectItem OuterShadowEffectItem = new OuterShadowEffectItem();

        public OutlineEffectItem OutlineEffectItem = new OutlineEffectItem();

        public EmbossEffectItem EmbossEffectItem = new EmbossEffectItem();
        public StraightenEffectItem StraightenEffectItem = new StraightenEffectItem();


        //@static
        public static ICanvasImage Render(EffectManager manager, ICanvasImage image)
        {
            if (manager.GaussianBlurEffectItem.IsOn) image = manager.GaussianBlurEffectItem.GetRender(image);
            if (manager.DirectionalBlurEffectItem.IsOn) image = manager.DirectionalBlurEffectItem.GetRender(image);
            if (manager.SharpenEffectItem.IsOn) image = manager.SharpenEffectItem.GetRender(image);
            if (manager.OuterShadowEffectItem.IsOn) image = manager.OuterShadowEffectItem.GetRender(image);

            if (manager.OutlineEffectItem.IsOn) image = manager.OutlineEffectItem.GetRender(image);

            if (manager.EmbossEffectItem.IsOn) image = manager.EmbossEffectItem.GetRender(image);
            if (manager.StraightenEffectItem.IsOn) image = manager.StraightenEffectItem.GetRender(image);

            return image;
        }
    }
}
