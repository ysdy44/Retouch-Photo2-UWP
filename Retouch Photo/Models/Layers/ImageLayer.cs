using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Retouch_Photo.Library;
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Models.Layers
{
    public class ImageLayer:Layer
    {
        
        public static string Type = "Image";
        protected ImageLayer() => base.Name = ImageLayer.Type;

        public CanvasBitmap Image { set; get; }


        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.Image,
                TransformMatrix = base.Transformer.Matrix* canvasToVirtualMatrix
            };
        }
        public override void ThumbnailDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Size controlSize)
        {
            try
            {

                ds.Clear(Windows.UI.Colors.Transparent);

                Matrix3x2 matrix = Layer.GetThumbnailMatrix(base.Transformer.Width, base.Transformer.Height, controlSize);

                ds.DrawImage(new Transform2DEffect
                {
                    Source = this.Image,
                    TransformMatrix = base.Transformer.Matrix * matrix
                });

            }
            catch (Exception)
            {
            }         
        }


        public static ImageLayer CreateFromBytes(ICanvasResourceCreator resourceCreator, byte[] bytes, int width, int height)
        {
            CanvasBitmap renderTarget = new CanvasRenderTarget(resourceCreator, width, height, 96);
            renderTarget.SetPixelBytes(bytes);

            return new ImageLayer
            {
                Transformer = Transformer.CreateFromSize(width, height, new Vector2(width / 2, height / 2)),
                Image = renderTarget
            };
        }

        public static ImageLayer CreateFromBitmap(ICanvasResourceCreator resourceCreator, CanvasBitmap bitmap, int width, int height)
        {
            return new ImageLayer
            {
                Transformer = Transformer.CreateFromSize(width, height, new Vector2(width / 2, height / 2)),
                Image = bitmap
            };
        }

        public static async Task<ImageLayer> CreateFromFlie(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            try
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream, 96);

                    int width = (int)bitmap.SizeInPixels.Width;
                    int height = (int)bitmap.SizeInPixels.Height;

                    return ImageLayer.CreateFromBitmap(resourceCreator, bitmap, width, height);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
