using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo.Models.Layers
{
    public class AcrylicLayer : Layer
    {
         
        public static string Type = "AcrylicLayer";
        protected AcrylicLayer() => base.Name = AcrylicLayer.Type;

        public VectorRect Rect;
        public float TintOpacity = 0.5f;
        public Color TintColor = Color.FromArgb(255, 255, 255, 255);
        public float BlurAmount = 12.0f;

        public override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 matrix)
        {
            return new CropEffect
            {
                SourceRectangle = Rect.Transform(matrix).ToRect(),
                Source = new BlendEffect
                {
                    Mode = BlendEffectMode.Screen,
                    Background = new GaussianBlurEffect
                    {
                        BlurAmount = this.BlurAmount,
                        Source = image
                    },
                    Foreground = new OpacityEffect
                    {
                        Opacity = this.TintOpacity,
                        Source = new ColorSourceEffect
                        {
                            Color = this.TintColor
                        }
                    }
                }
            };
        }

        public override VectorRect GetBoundRect(ICanvasResourceCreator creator)
        {
            return this.Rect;
        }

        public static AcrylicLayer CreateFromRect(ICanvasResourceCreator creator, VectorRect rect, Color color, float opacity = 0.5f)
        {
            return new AcrylicLayer
            {
                Rect = rect,
                TintColor = color,
                TintOpacity=opacity
            };
        }



    }
}
