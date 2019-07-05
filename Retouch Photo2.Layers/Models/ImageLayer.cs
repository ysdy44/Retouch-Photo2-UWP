using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Controls;
using FanKit.Transformers;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : Layer
    {
        /// <summary> <see cref = "ImageLayer" />'s bitmap. </summary>
        public CanvasBitmap Bitmap { set; get; }

        //@Construct
        public ImageLayer()
        {
            base.Name = "Image";
        }

        //@Override
        public override UIElement GetIcon() => new ImageControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            //@Debug
            CanvasBitmap bitmap = this.Bitmap;

            return new ImageLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                Bitmap = bitmap,
            };
        }
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.Bitmap,
                TransformMatrix = base.GetMatrix() * canvasToVirtualMatrix
            };
        }

        //@Static
        /// <summary>
        /// Create a ImageLayer from file.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="file"> file </param>
        /// <returns> ImageLayer </returns>
        public static async Task<ImageLayer> CreateFromFlie(ICanvasResourceCreator resourceCreator, StorageFile file)
        {
            try
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream, 96);

                    return ImageLayer.CreateFromBitmap(bitmap); 
                }
            }
            catch (Exception) { return null; }
        }
        /// <summary>
        /// Create a ImageLayer from bitmap.
        /// </summary>
        /// <param name="bitmap"> bitmap </param>
        /// <returns> ImageLayer </returns>
        public static ImageLayer CreateFromBitmap(CanvasBitmap bitmap)
        {
            int width = (int)bitmap.SizeInPixels.Width;
            int height = (int)bitmap.SizeInPixels.Height;

            Transformer transformer = new Transformer(width, height, Vector2.Zero);

            return new ImageLayer
            {
                Source = transformer,
                Destination = transformer,
                DisabledRadian = false,

                Bitmap = bitmap
            };
        }

    }
}