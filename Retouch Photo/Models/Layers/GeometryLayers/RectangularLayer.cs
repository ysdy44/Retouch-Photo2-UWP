using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Controls.LayerControls.GeometryControls;
using System.Numerics;
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
            base.Icon = new RectangularControl();
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
