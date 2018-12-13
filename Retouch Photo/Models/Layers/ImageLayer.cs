using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public class ImageLayer:Layer
    {
        
        public static string Type = "ImageLayer";
        protected ImageLayer() => base.Name = ImageLayer.Type;

        public CanvasBitmap Image { set; get; }

        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 matrix)
        {
            return new Transform2DEffect
            {
                Source = Image,
                TransformMatrix = matrix
            };
        }
        public override VectorRect GetBoundRect(ICanvasResourceCreator creator) => new VectorRect(this.Image.Bounds);


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
