using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using Windows.Foundation;
using Windows.UI;

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
        public override void CurrentDraw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.GetBoundRect(viewModel.CanvasControl), viewModel.Transformer.Matrix);
        }
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return VectorRect.CreateFormRect(this.CanvasRenderTarget.Bounds);
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
