using FanKit.Transformers;
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

        public NodeCollection NodeCollection { get; private set; }


        //@Construct
        /// <summary>
        /// Construct a curve layer.
        /// </summary>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(IEnumerable<Node> nodes) => this.NodeCollection = new NodeCollection(nodes); 
        /// <summary>
        /// Construct a curve layer from a line.
        /// </summary>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(Vector2 left, Vector2 right) => this.NodeCollection = new NodeCollection(left, right);


        //@Override
        public override void CacheTransform()
        {
            base.CacheTransform();

            for (int i = 0; i < this.NodeCollection.Count; i++)
            {
                Node node = this.NodeCollection[i];
                node.CacheTransform();
                this.NodeCollection[i] = node;
            }
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);

            for (int i = 0; i < this.NodeCollection.Count; i++)
            {
                Node node = this.NodeCollection[i];
                node.TransformMultiplies(matrix);
                this.NodeCollection[i] = node;
            }
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);

            for (int i = 0; i < this.NodeCollection.Count; i++)
            {
                Node node = this.NodeCollection[i];
                node.TransformAdd(vector);
                this.NodeCollection[i] = node;
            }
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {  
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(this.NodeCollection.First().Point);

            for (int i = 0; i < this.NodeCollection.Count - 1; i++)
            {
                Node current = this.NodeCollection[i];             
                Node preview = this.NodeCollection[i + 1];
                
                if (current.IsSmooth && preview.IsSmooth)
                    pathBuilder.AddCubicBezier(current.LeftControlPoint, preview.RightControlPoint, preview.Point);
                else if (current.IsSmooth && preview.IsSmooth == false)
                    pathBuilder.AddCubicBezier(current.LeftControlPoint, preview.Point, preview.Point);
                else if (current.IsSmooth == false && preview.IsSmooth)
                    pathBuilder.AddCubicBezier(current.Point, preview.RightControlPoint, preview.Point);
                else 
                    pathBuilder.AddLine(preview.Point);
            }

            pathBuilder.EndFigure(CanvasFigureLoop.Open);
            return CanvasGeometry.CreatePath(pathBuilder).Transform(canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new CurveLayer(this.NodeCollection)
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


        //@Static
        /// <summary>
        /// Correct transformer based on nodes.
        /// </summary>
        public void CorrectionTransformer()
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            foreach (Node node in this.NodeCollection)
            {
                Vector2 vector = node.Point;

                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }
            
            Transformer transformer = new Transformer(left, top, right, bottom);
            base.Source = transformer;
            base.Destination = transformer;
        }
    }
}