using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Controls;
using Retouch_Photo2.Layers.ILayer;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s EllipseLayer .
    /// </summary>
    public class EllipseLayer : IGeometryLayer
    {
        //@Construct
        public EllipseLayer()
        {
            base.Name = "Ellipse";
        }

        //@Override
        public override UIElement GetIcon() => new EllipseControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new EllipseLayer
            {
                Name=this.Name,
                Opacity=this.Opacity,
                BlendType=this.BlendType,
                TransformerMatrix=this.TransformerMatrix,

                IsChecked = this.IsChecked,
                Visibility = this.Visibility,

                FillColor = base.FillColor,
            };
        }         

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
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
            Vector2 left = Vector2.Transform(base.TransformerMatrix.Destination.CenterLeft, canvasToVirtualMatrix);
            Vector2 top = Vector2.Transform(base.TransformerMatrix.Destination.CenterTop, canvasToVirtualMatrix);
            Vector2 right = Vector2.Transform(base.TransformerMatrix.Destination.CenterRight, canvasToVirtualMatrix);
            Vector2 bottom = Vector2.Transform(base.TransformerMatrix.Destination.CenterBottom, canvasToVirtualMatrix);
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
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(bottom);
            pathBuilder.AddCubicBezier(bottom1, left2, left);
            pathBuilder.AddCubicBezier(left1, top2, top);
            pathBuilder.AddCubicBezier(top1, right2, right);
            pathBuilder.AddCubicBezier(right1, bottom2, bottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }
    }
}