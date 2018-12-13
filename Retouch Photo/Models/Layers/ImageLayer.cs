using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public class ImageLayer:Layer
    {
        
        protected ImageLayer()
        {
        }

        public static string ID = "ImageLayer";

        public CanvasBitmap Image { set; get; }

        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image)
        {
            return this.Image;
        }
        public override void CurrentDraw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.GetBoundRect(viewModel.CanvasControl), viewModel.Transformer.Matrix);
        }
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return VectorRect.CreateFormRect(this.Image.Bounds);
        }


        

        public static ImageLayer CreateFromBytes(ICanvasResourceCreatorWithDpi resourceCreator, byte[] bytes, int width, int height)
        {
            CanvasBitmap renderTarget = new CanvasRenderTarget(resourceCreator, width, height, 96);
            renderTarget.SetPixelBytes(bytes);

            return new ImageLayer
            {
                Image = renderTarget
            };
        }

        public static ImageLayer CreateFromBitmap(ICanvasResourceCreatorWithDpi resourceCreator, CanvasBitmap bitmap, int width, int height)
        { 
            return new ImageLayer
            {
                Image = bitmap
            };
        }


    }
}
