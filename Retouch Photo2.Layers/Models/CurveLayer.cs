using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s CurveLayer .
    /// </summary>
    public class CurveLayer : IGeometryLayer
    {
        //@Override       
        public override string Type => "Curve";
        public override UIElement Icon => new CurveControl();

        public List<Node> Nodes { get; private set; } = new List<Node>();


        //@Construct
        /// <summary>
        /// Construct a curve layer.
        /// </summary>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(IEnumerable<Node> nodes) => this.Nodes = nodes.ToList();
        /// <summary>
        /// Construct a curve layer from a line.
        /// </summary>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(Vector2 left, Vector2 right) => this.Nodes = new List<Node>
        {
             new Node(left),
             new Node(right),
        };
               

        //@Override
        public override void CacheTransform()
        {
            base.CacheTransform();

            foreach (Node node in this.Nodes)
            {
                node.CacheTransform();
            }
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);

            foreach (Node node in this.Nodes)
            {
                node.TransformMultiplies(matrix);
            }
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);

            foreach (Node node in this.Nodes)
            {
                node.TransformAdd(vector);
            }
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {  
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(this.Nodes.First().Vector);

            for (int i = 0; i < this.Nodes.Count - 1; i++)
            {
                Vector2 controlPoint1 = this.Nodes[i].LeftControl;
                Vector2 controlPoint2 = this.Nodes[i + 1].RightControl;
                Vector2 endPoint = this.Nodes[i + 1].Vector;
                pathBuilder.AddCubicBezier(controlPoint1, controlPoint2, endPoint);
            }

            pathBuilder.EndFigure(CanvasFigureLoop.Open);
            return CanvasGeometry.CreatePath(pathBuilder).Transform(canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new CurveLayer(this.Nodes)
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };
        }
    }
}