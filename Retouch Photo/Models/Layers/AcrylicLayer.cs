using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.LayerControls;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Models.Layers
{
    public class AcrylicLayer : Layer
    {
         
        public static readonly string Type = "Acrylic";

        public float TintOpacity = 0.5f;
        public Color TintColor = Color.FromArgb(255, 255, 255, 255);
        public float BlurAmount = 12.0f;

        protected AcrylicLayer()
        {
            base.Name = AcrylicLayer.Type;
            base.Icon = new AcrylicControl();
        }

        public override void ColorChanged(Color color, bool fillOrStroke)
        {
            this.TintColor = color;
        }


        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);

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

        
        public static AcrylicLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color, float opacity = 0.5f)
        {
            return new AcrylicLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y), disabledRadian: true),
                TintColor = color,
                TintOpacity = opacity
            };
        }

    }
}
