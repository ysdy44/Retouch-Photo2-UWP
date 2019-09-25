using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Icons;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s AcrylicLayer .
    /// </summary>
    public class AcrylicLayer : Layer
    {
        public float TintOpacity = 0.5f;
        public Color TintColor = Color.FromArgb(255, 255, 255, 255);
        public float BlurAmount = 12.0f;
        

        //@Override
        public override string Type => "Acrylic";
        public override UIElement Icon => new AcrylicIcon();
        public override Color? FillColor
        {
           get => this.TintColor;
            set
            {
                if (value is Color color)
                {
                    this.TintColor = color;
                }
            }
        }
        public static Action<string> ddddddd;

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);

            return new CropEffect
            {
                SourceRectangle = new Rect(leftTop.ToPoint(), rightBottom.ToPoint()),
                Source = new CompositeEffect
                {
                    Sources =
                    {
                        new GaussianBlurEffect
                        {
                             BlurAmount = this.BlurAmount,
                             Source = previousImage
                        },
                        new OpacityEffect
                        {
                            Opacity = this.TintOpacity,
                            Source = new ColorSourceEffect
                            {
                                Color = this.TintColor
                             }
                         }
                    }
                }
            };
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            AcrylicLayer acrylicLayer = new AcrylicLayer
            {
                TintOpacity = this.TintOpacity,
                TintColor = this.TintColor,
                BlurAmount = this.BlurAmount,
            };

            base.CopyWith(resourceCreator, acrylicLayer);

            return acrylicLayer;
        }
    }
}