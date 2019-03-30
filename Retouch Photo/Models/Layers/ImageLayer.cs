using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.LayerControls;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Effects;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Models.Layers
{
    public class ImageLayer:Layer
    {
        
        public static readonly string Type = "Image";

        public CanvasBitmap Image { set; get; }
        

        protected ImageLayer()
        {
            base.Name = ImageLayer.Type;
            base.Icon = new ImageControl();
        }

        private UIElement ImageControl()
        {
            throw new NotImplementedException();
        }

        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.Image,
                TransformMatrix = base.Transformer.Matrix* canvasToVirtualMatrix
            };
        }


        public static ImageLayer CreateFromBytes(ICanvasResourceCreator resourceCreator, byte[] bytes, int width, int height)
        {
            CanvasBitmap renderTarget = new CanvasRenderTarget(resourceCreator, width, height, 96);
            renderTarget.SetPixelBytes(bytes);

            return new ImageLayer
            {
                Transformer = Transformer.CreateFromSize(width, height, Vector2.Zero),
                Image = renderTarget
            };
        }
        public static ImageLayer CreateFromBytes(ICanvasResourceCreator resourceCreator, byte[] bytes, int width, int height, Vector2 center)
        {
            CanvasBitmap renderTarget = new CanvasRenderTarget(resourceCreator, width, height, 96);
            renderTarget.SetPixelBytes(bytes);

            return new ImageLayer
            {
                Transformer = Transformer.CreateFromSize(width, height, new Vector2(center.X - width / 2, center.Y - height)),
                Image = renderTarget
            };
        }

        public static ImageLayer CreateFromBitmap(CanvasBitmap bitmap, Transformer transformer)=> new ImageLayer
        {
            Transformer = transformer,
            Image = bitmap
        };
        public static ImageLayer CreateFromBitmap(CanvasBitmap bitmap, Vector2 center)
        {
            int width = (int)bitmap.SizeInPixels.Width;
            int height = (int)bitmap.SizeInPixels.Height;

            return new ImageLayer
            {
                Transformer = Transformer.CreateFromSize(width, height, new Vector2(center.X - width / 2, center.Y - height/2)),
                Image = bitmap
            };
        }

        public static async Task<ImageLayer> CreateFromFlie(ICanvasResourceCreator resourceCreator, StorageFile file, Vector2 center)
        {
            try
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream, 96);

                    return ImageLayer.CreateFromBitmap( bitmap, center);
                }
            }
            catch (Exception){return null;}
        }


    }
}
