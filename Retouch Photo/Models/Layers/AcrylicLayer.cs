using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Models.Layers
{
    public class AcrylicLayer : Layer
    {
         
        public static string Type = "AcrylicLayer";
        protected AcrylicLayer() => base.Name = AcrylicLayer.Type;

        public float TintOpacity = 0.5f;
        public Color TintColor = Color.FromArgb(255, 255, 255, 255);
        public float BlurAmount = 12.0f;

        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Vector2 point0 = Vector2.Transform(this.Transformer.Postion, canvasToVirtualMatrix);
            Vector2 point1 = Vector2.Transform(new Vector2(this.Transformer.Postion.X + this.Transformer.Width, this.Transformer.Postion.Y + this.Transformer.Height), canvasToVirtualMatrix);

            return new CropEffect
            {
                SourceRectangle = new Rect(point0.ToPoint(), point1.ToPoint()),
                Source = new CompositeEffect
                {
                    Sources =
                    {
                        new GaussianBlurEffect
                        {
                             BlurAmount = this.BlurAmount,
                             Source = image
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


        public static AcrylicLayer CreateFromRect(ICanvasResourceCreator creator, Rect rect, Color color, float opacity = 0.5f)
        {
            return new AcrylicLayer
            {
                Transformer = Transformer.CreateFromRect(rect, disabledRadian: true),
                TintColor = color,
                TintOpacity = opacity
            };
        }

    }
}
