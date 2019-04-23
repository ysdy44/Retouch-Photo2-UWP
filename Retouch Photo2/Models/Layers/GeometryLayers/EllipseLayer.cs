using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Controls.LayerControls.GeometryControls;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;
using Retouch_Photo2.Brushs;

namespace Retouch_Photo2.Models.Layers.GeometryLayers
{
    public class EllipseLayer : GeometryLayer
    {

        public static readonly string Type = "Ellipse";
        protected EllipseLayer()
        {
            base.Name = EllipseLayer.Type;
            base.Icon = new EllipseControl();
        }
             
        protected override CanvasGeometry GetGeometry(ICanvasResourceCreator creator, Matrix3x2 canvasToVirtualMatrix)
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

            //LTRB
            Vector2 left = Vector2.Transform(this.Transformer.DstLeft, canvasToVirtualMatrix);
            Vector2 top = Vector2.Transform(this.Transformer.DstTop, canvasToVirtualMatrix);
            Vector2 right = Vector2.Transform(this.Transformer.DstRight, canvasToVirtualMatrix);
            Vector2 bottom = Vector2.Transform(this.Transformer.DstBottom, canvasToVirtualMatrix);
            //HV
            Vector2 horizontal = (right - left) * 0.276f;// vector / 2 * 0.552f
            Vector2 vertical = (bottom - top) * 0.276f;// vector / 2 * 0.552f

            //Control
            Vector2 left1 = left - vertical;
            Vector2 left2 = left + vertical;
            Vector2 top1 = top + horizontal;
            Vector2 top2 = top - horizontal;
            Vector2 right1 = right + vertical;
            Vector2 right2 = right - vertical;
            Vector2 bottom1 = bottom - horizontal;
            Vector2 bottom2 = bottom + horizontal;

            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(creator);
            pathBuilder.BeginFigure(bottom);
            pathBuilder.AddCubicBezier(bottom1, left2, left);
            pathBuilder.AddCubicBezier(left1, top2, top);
            pathBuilder.AddCubicBezier(top1, right2, right);
            pathBuilder.AddCubicBezier(right1, bottom2, bottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }
                 
        public static EllipseLayer CreateFromRect(ICanvasResourceCreator creator, VectRect rect, Color color)
        {
            return new EllipseLayer
            {
                Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y)),
                FillBrush = new Brush
                {
                    Type = BrushType.Color,
                    Color = color
                }
            };
        }
    }
}
