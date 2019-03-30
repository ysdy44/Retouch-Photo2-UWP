using Microsoft.Graphics.Canvas;
using Retouch_Photo.Tools.Controls;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Models.Layers
{
    public class LineLayer : Layer
    {

        public static readonly string Type = "Line";
        protected LineLayer()
        {
            base.Name = LineLayer.Type;
            base.Icon = new LineControl();
        }

        public Color Stroke = Color.FromArgb(255, 255, 255, 255);
        public float StrokeWidth = 1.0f;

        //@Override     
        public override void ColorChanged(Color value)
        {
            this.Stroke = value;
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
        public override void ThumbnailDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Size controlSize)
        {/*
            ds.Clear(Windows.UI.Colors.Transparent);

            Rect rect = Layer.GetThumbnailSize(base.Transformer.Width, base.Transformer.Height, controlSize);

            ds.FillRectangle(rect, this.TintColor);
            */
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

