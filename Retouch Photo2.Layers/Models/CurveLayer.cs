using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
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

        public NodeCollection NodeCollection { get; private set; }


        //@Construct
        /// <summary>
        /// Construct a curve layer.
        /// </summary>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(LayerCollection layerCollection,IEnumerable<Node> nodes) : base(layerCollection)
        {
            base.Control.Icon = new CurveIcon();
            this.NodeCollection = new NodeCollection(nodes); 
        }
        /// <summary>
        /// Construct a curve layer from a line.
        /// </summary>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(LayerCollection layerCollection,Vector2 left, Vector2 right) : base(layerCollection)
        {
            base.Control.Icon = new CurveIcon();
            this.NodeCollection = new NodeCollection(left, right);
        }


        //@Override
        public override void CacheTransform()
        {
            base.CacheTransform();

            this.NodeCollection.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);

            this.NodeCollection.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);

            this.NodeCollection.TransformAdd(vector);
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return this.NodeCollection.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);
        }

        public override ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator)
        {
            CurveLayer curveLayer = new CurveLayer(layerCollection, this.NodeCollection)
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
                NodeCollection = new NodeCollection(from node in this.NodeCollection select node)
            };

            LayerBase.CopyWith(layerCollection, resourceCreator, curveLayer, this);
            return curveLayer;
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

            base.TransformManager.Source = transformer;
            base.TransformManager.Destination = transformer;
        }
    }
}