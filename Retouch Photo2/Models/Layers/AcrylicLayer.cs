using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Controls.LayerControls;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Models.Layers
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

        public override void Draw(CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.Transformer.DstRightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.Transformer.DstLeftBottom, matrix);

            ds.DrawLine(leftTop, rightTop, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Windows.UI.Colors.DodgerBlue);
        }
        protected override ICanvasImage GetRender(IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
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
