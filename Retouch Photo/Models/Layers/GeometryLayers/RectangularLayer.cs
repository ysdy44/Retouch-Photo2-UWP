using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {

        public static readonly string Type = "Rectangular";
        protected RectangularLayer() => base.Name = RectangularLayer.Type;

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
            Matrix3x2 matrix = this.Transformer.Matrix * canvasToVirtualMatrix;
            Vector2 leftTop = this.Transformer.TransformLeftTop(matrix);
            Vector2 rightTop = this.Transformer.TransformRightTop(matrix);
            Vector2 rightBottom = this.Transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = this.Transformer.TransformLeftBottom(matrix);
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
            ds.Clear(Colors.Transparent);

            Rect rect = Layer.GetThumbnailSize(base.Transformer.Width, base.Transformer.Height, controlSize);

            if (this.IsFill) ds.FillRectangle(rect, base.FillBrush);
            if (this.IsStroke) ds.DrawRectangle(rect, base.StrokeBrush, base.StrokeWidth);
        }



        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new RectangularLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, rect.Center),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }
    

    }
}
