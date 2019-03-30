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
    public class EllipseLayer : GeometryLayer
    {

        public static readonly string Type = "Ellipse";
        protected EllipseLayer()
        {
            base.Name = EllipseLayer.Type;
            base.Icon = new EllipseControl();
        }
        
        protected override ICanvasImage GetRender(ICanvasResourceCreator creator, IGraphicsEffectSource image, Matrix3x2 canvasToVirtualMatrix)
        {
            /// <summary>
            /// A Ellipse has left, top, right, bottom four nodes.
            /// 
            /// Control points on the left and right sides of the node.
            /// 
            /// The distance of the control point 
            /// is 0.552f times
            /// the length of the square edge.
            /// <summary>
            Vector2 left = Vector2.Transform(this.Transformer.DstLeft, canvasToVirtualMatrix);
            Vector2 top = Vector2.Transform(this.Transformer.DstTop, canvasToVirtualMatrix);
            Vector2 right = Vector2.Transform(this.Transformer.DstRight, canvasToVirtualMatrix);
            Vector2 bottom = Vector2.Transform(this.Transformer.DstBottom, canvasToVirtualMatrix);

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
    
        public static EllipseLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new EllipseLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y)),
                FillBrush = new CanvasSolidColorBrush(creator, color)
            };
        }
    }
}
