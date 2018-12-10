using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Models.Layers
{
    public class PixelLayer:Layer
    {
        public static string ID = "PixelLayer";

        public CanvasRenderTarget CanvasRenderTarget { set; get; }

        public override ICanvasImage GetRender(ICanvasResourceCreator creator)
        {
            return this.CanvasRenderTarget;
        }

        public static Layer CreateFromSize(ICanvasResourceCreatorWithDpi resourceCreator, int width, int height)
        {
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(resourceCreator, width, height);

            return new PixelLayer
            {
                CanvasRenderTarget = renderTarget
            };
        }
    }
}
