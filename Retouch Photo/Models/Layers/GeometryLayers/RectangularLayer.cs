using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Tools.Controls;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {

        public static readonly string Type = "Rectangular";
        protected RectangularLayer()
        {
            base.Name = RectangularLayer.Type;
            base.Icon = new RectangleControl();
        }

        //@Override     
        public override void ColorChanged(Color value)
        {
            if (base.FillBrush is CanvasSolidColorBrush brush)
            {
                brush.Color = value;
            }
        }
        public override void BrushChanged(ICanvasBrush brush)
        {
            base.FillBrush = brush;
        }

        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {            
            Vector2 leftTop = Vector2.Transform(this.Transformer.DstLeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(this.Transformer.DstRightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(this.Transformer.DstRightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(this.Transformer.DstLeftBottom, canvasToVirtualMatrix);

            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(creator, new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            });

            CanvasCommandList command = new CanvasCommandList(creator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                if (this.IsFill) ds.FillGeometry(geometry, base.FillBrush);
                if (this.IsStroke) ds.DrawGeometry(geometry, base.StrokeBrush, base.StrokeWidth);
            }
            return command;
        }
        public override void ThumbnailDraw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Size controlSize)
        {
            /*
             ds.Clear(Colors.Transparent);

            Rect rect = Layer.GetThumbnailSize(base.Transformer.Width, base.Transformer.Height, controlSize);

            if (this.IsFill) ds.FillRectangle(rect, base.FillBrush);
            if (this.IsStroke) ds.DrawRectangle(rect, base.StrokeBrush, base.StrokeWidth);
             */
        }



        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new RectangularLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y)),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }
    

    }
}
