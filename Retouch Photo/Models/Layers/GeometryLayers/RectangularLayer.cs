using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
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

namespace Retouch_Photo.Models.Layers.GeometryLayers
{
    public class RectangularLayer:GeometryLayer
    {

        public static string Type = "Rectangular";
        protected RectangularLayer() => base.Name = RectangularLayer.Type;

        Vector2[] points = new Vector2[4];

        //@Override     
        public override void ColorChanged(Color value)
        {
            if (base.FillBrush is CanvasSolidColorBrush brush)
            {
                brush.Color = value;
            }
        }
        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 matrix = this.Transformer.Matrix * canvasToVirtualMatrix;
            this.points[0] = this.Transformer.TransformLeftTop(matrix);
            this.points[1] = this.Transformer.TransformRightTop(matrix);
            this.points[2] = this.Transformer.TransformRightBottom(matrix);
            this.points[3] = this.Transformer.TransformLeftBottom(matrix);
            CanvasGeometry geometry= CanvasGeometry.CreatePolygon(creator,this.points);

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
            ds.Clear(Windows.UI.Colors.Transparent);

            Rect rect = Layer.GetThumbnailSize(base.Transformer.Width, base.Transformer.Height, controlSize);

            if (this.IsFill) ds.FillRectangle(rect, base.FillBrush);
            if (this.IsStroke) ds.DrawRectangle(rect, base.StrokeBrush, base.StrokeWidth);
        }



        public static RectangularLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new RectangularLayer
            {
                Transformer = Transformer.CreateFromRect(rect),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }
    

    }
}
