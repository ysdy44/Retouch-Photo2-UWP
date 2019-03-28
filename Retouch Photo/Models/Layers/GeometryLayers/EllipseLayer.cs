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
    public class EllipseLayer : GeometryLayer
    {

        public static readonly string Type = "Ellipse";
        protected EllipseLayer() => base.Name = EllipseLayer.Type;
        
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
                       
            Vector2 left = this.Transformer.TransformLeft(matrix);
            Vector2 top = this.Transformer.TransformTop(matrix);
            Vector2 right = this.Transformer.TransformRight(matrix);
            Vector2 bottom = this.Transformer.TransformBottom(matrix);

            Vector2 horizontal = (right - left) * 0.276f;// vector / 2 * 0.552f
            Vector2 vertical = (bottom - top) * 0.276f;// vector / 2 * 0.552f
                            
            Vector2 left1 = left - vertical;
            Vector2 left2 = left + vertical; 
            Vector2 top1 =top + horizontal;
            Vector2 top2 = top - horizontal;
            Vector2 right1 = right + vertical;
            Vector2 right2 = right - vertical;
            Vector2 bottom1 =bottom - horizontal; 
            Vector2 bottom2 = bottom + horizontal;

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(creator);
            pathBuilder.BeginFigure(bottom);
            pathBuilder.AddCubicBezier(bottom1, left2, left);
            pathBuilder.AddCubicBezier(left1, top2, top);
            pathBuilder.AddCubicBezier(top1, right2, right);
            pathBuilder.AddCubicBezier(right1, bottom2, bottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder);
            
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



        public static EllipseLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new EllipseLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, rect.Center),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }


    }
}
