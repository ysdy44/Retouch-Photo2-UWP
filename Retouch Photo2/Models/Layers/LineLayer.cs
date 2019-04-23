using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;
using Retouch_Photo2.Controls.LayerControls;

namespace Retouch_Photo2.Models.Layers
{
    public class LineLayer : Layer
    {

        public static readonly string Type = "Line";

        public Color Stroke = Color.FromArgb(255, 255, 255, 255);
        public float StrokeWidth = 1.0f;

        protected LineLayer()
        {
            base.Name = LineLayer.Type;
            base.Icon = new LineControl();
        }


        public override void ColorChanged(Color color, bool fillOrStroke)
        {
            this.Stroke = color;
        }


        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);

            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                ds.DrawLine(leftTop, rightBottom, this.Stroke, this.StrokeWidth);
            }
            return command;
        }


        public static LineLayer CreateFromRect(ICanvasResourceCreator creator, Vector2 leftTop, Vector2 rightBottom, Color stroke, float strokeWidth = 1f)
        {
            return new LineLayer
            {
                Transformer = Transformer.CreateFromVector(leftTop, rightBottom),
                Stroke = stroke,
                StrokeWidth = strokeWidth
            };
        }

    }
}

