using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;
using Retouch_Photo2.Controls.LayerControls;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Models.Layers
{
    public class LineLayer : Layer
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public static readonly string Type = "Line";

        Vector2 StartPoint, OldStartPoint;
        Vector2 EndPoint, OldEndPoint;

        public Color Stroke = Color.FromArgb(255, 255, 255, 255);
        public float StrokeWidth = 1.0f;

        protected LineLayer()
        {
            base.Name = LineLayer.Type;
            base.Icon = new LineControl();
        }

        public override void TransformStart()
        {
            this.OldStartPoint = this.StartPoint;
            this.OldEndPoint = this.EndPoint;
        }
        public override void TransformDelta()
        {
            Matrix3x2 matrix = Transformer.DivideMatrix(base.OldTransformer, base.Transformer);
 
            this.StartPoint = Vector2.Transform(this.OldStartPoint, matrix);
            this.EndPoint = Vector2.Transform(this.OldEndPoint, matrix);
        }
        public override void TransformComplete()
        {
            this.TransformDelta();
            base.Transformer = Transformer.CreateFromVector(this.StartPoint, this.EndPoint);
        }

        public override void ColorChanged(Color color, bool fillOrStroke)
        {
            this.Stroke = color;
        }

        protected override ICanvasImage GetRender(IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);

            CanvasCommandList command = new CanvasCommandList(this.ViewModel.CanvasDevice);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                ds.DrawLine(leftTop, rightBottom, this.Stroke, this.StrokeWidth);
            }
            return command;
        }


        public static LineLayer CreateFromRect(ICanvasResourceCreator creator, Vector2 startPoint, Vector2 endPoint, Color stroke, float strokeWidth = 1f)
        {
            return new LineLayer
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                Transformer = Transformer.CreateFromVector(startPoint, endPoint),
                Stroke = stroke,
                StrokeWidth = strokeWidth
            };
        }

    }
}

