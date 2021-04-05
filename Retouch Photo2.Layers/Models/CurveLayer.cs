// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s CurveLayer .
    /// </summary>
    public partial class CurveLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Curve;

        //@Construct
        /// <summary>
        /// Initializes a curve-layer.
        /// 
        /// Warning, 
        /// please do not call it.
        /// Because this constructor is only for the reflection of <see cref="Retouch_Photo2.Layers.XML.LoadILayer(XElement)"/>, 
        /// </summary>
        public CurveLayer() { }
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>     
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(IEnumerable<Node> nodes) => base.Nodes = new NodeCollection(nodes);
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>     
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(NodeCollection nodes) => base.Nodes = nodes;
        /// <summary>
        /// Initializes a curve-layer from a line.
        /// </summary>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(Vector2 left, Vector2 right) => base.Nodes = new NodeCollection(left, right);


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;


                //@Release
                float left = float.MaxValue;
                float top = float.MaxValue;
                float right = float.MinValue;
                float bottom = float.MinValue;

                foreach (Node node in base.Nodes)
                {
                    Vector2 vector = node.Point;

                    switch (node.Type)
                    {
                        case NodeType.BeginFigure:
                        case NodeType.Node:
                            {
                                if (left > vector.X) left = vector.X;
                                if (top > vector.Y) top = vector.Y;
                                if (right < vector.X) right = vector.X;
                                if (bottom < vector.Y) bottom = vector.Y;
                            }
                            break;
                    }
                }
                
                Transformer transformer = new Transformer(left, top, right, bottom);
                //@Release


                //TransformerBorder border = new TransformerBorder(base.Nodes);
                //Transformer transformer = border.ToTransformer();


                this.Transform.Transformer = transformer;
                return this.Transform.IsCrop ? this.Transform.CropTransformer : transformer;
            }

            return this.Transform.IsCrop ? this.Transform.CropTransformer : this.Transform.Transformer;
        }

        public override void CacheTransform()
        {
            base.CacheTransform();
            base.Nodes.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
            base.Nodes.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
            base.Nodes.TransformAdd(vector);
        }


        public override ILayer Clone() => LayerBase.CopyWith(this, new CurveLayer
        {
            Nodes = base.Nodes.Clone()
        });

        public override void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveNodeCollection("Nodes", base.Nodes));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Nodes") is XElement nodes) base.Nodes = FanKit.Transformers.XML.LoadNodeCollection(nodes);
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            return base.Nodes.CreateGeometry(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            return base.Nodes.CreateGeometry(resourceCreator).Transform(matrix);
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;
        
    }
}