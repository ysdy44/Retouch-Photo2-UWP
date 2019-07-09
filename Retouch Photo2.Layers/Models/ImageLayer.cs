using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Controls;
using System;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : Layer
    {
        /// <summary> <see cref = "ImageLayer" />'s image key. </summary>
        public string ImageKey { get; protected set; }

        /// <summary> <see cref = "ImageLayer" />'s image function. </summary>
        public Func<string, CanvasBitmap> GetImage { get; protected set; }

        //@Construct
        public ImageLayer()
        {
            base.Name = "Image";
        }
        public ImageLayer(string BitmapKey, Func<string, CanvasBitmap> GetBitmap)
        {
            base.Name = "Image";
            this.ImageKey = BitmapKey;
            this.GetImage = GetBitmap;
            
            //Image
            CanvasBitmap bitmap = this.GetImage(this.ImageKey);
            int width = (int)bitmap.SizeInPixels.Width;
            int height = (int)bitmap.SizeInPixels.Height;

            //Transformer
            Transformer transformer = new Transformer(width, height, Vector2.Zero);
            base.Source = transformer;
            base.Destination = transformer;
            base.DisabledRadian = false;
        }

        //@Override
        public override UIElement GetIcon() => new ImageControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
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

                ImageKey = this.ImageKey,
                GetImage = this.GetImage,
            };
        }
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            //Image
            CanvasBitmap bitmap = this.GetImage(this.ImageKey);

            return new Transform2DEffect
            {
                Source = bitmap,
                TransformMatrix = base.GetMatrix() * canvasToVirtualMatrix
            };
        }
    }
}