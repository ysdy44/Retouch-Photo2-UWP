using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s CurveLayer .
    /// </summary>
    public partial class CurveLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Curve;

        //@Content 
        public NodeCollection Nodes { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>  
        public CurveLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(CanvasDevice customDevice, IEnumerable<Node> nodes) : this(customDevice) => this.Nodes = new NodeCollection(nodes);
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(CanvasDevice customDevice, NodeCollection nodes) : this(customDevice) => this.Nodes = nodes;
        /// <summary>
        /// Initializes a curve-layer from a line.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(CanvasDevice customDevice, Vector2 left, Vector2 right) : this(customDevice) => this.Nodes = new NodeCollection(left, right);


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                TransformerBorder border = new TransformerBorder(this.Nodes);
                Transformer transformer = border.ToTransformer();
                this.Transform.Transformer = transformer;
                return this.Transform.IsCrop ? this.Transform.CropTransformer : transformer;
            }

            return this.Transform.IsCrop ? this.Transform.CropTransformer : this.Transform.Transformer;
        }

        public override void CacheTransform()
        {
            base.CacheTransform();
            this.Nodes.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
            this.Nodes.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
            this.Nodes.TransformAdd(vector);
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            CurveLayer curveLayer = new CurveLayer(customDevice)
            {
                Nodes = this.Nodes.Clone()
            };

            LayerBase.CopyWith(customDevice, curveLayer, this);
            return curveLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveNodeCollection("Nodes", this.Nodes));
        }
        public override void Load(XElement element)
        {
            this.Nodes = FanKit.Transformers.XML.LoadNodeCollection(element.Element("Nodes"));
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            return this.Nodes.CreateGeometry(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return this.Nodes.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);
        }
    }

    /// <summary>
    /// <see cref="LayerBase"/>'s CurveLayer .
    /// </summary>
    public partial class CurveLayer : LayerBase, ILayer
    {

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Curve");
        }

        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;

        public override void DrawNode(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            drawingSession.DrawNodeCollection(this.Nodes, matrix, accentColor);
        }

        public override NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, Matrix3x2 matrix)
        {
            return NodeCollection.ContainsNodeCollectionMode(point, this.Nodes, matrix);
        }

        public override void NodeCacheTransform() => this.Nodes.CacheTransform();
        public override void NodeTransformMultiplies(Matrix3x2 matrix) => this.Nodes.TransformMultipliesOnlySelected(matrix);
        public override void NodeTransformAdd(Vector2 vector) => this.Nodes.TransformAddOnlySelected(vector);

        public override bool NodeSelectionOnlyOne(Vector2 point, Matrix3x2 matrix) => this.Nodes.SelectionOnlyOne(point, matrix);

        public override void NodeBoxChoose(TransformerRect boxRect) => this.Nodes.BoxChoose(boxRect);

        public override void NodeMovePoint(Vector2 point)
        {
            Node node = this.Nodes.SelectedItem;
            Node.Move(point, node);
        }
        public override void NodeControllerControlPoint(SelfControlPointMode mode, EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 point, bool isLeftControlPoint)
        {
            Node node = this.Nodes.SelectedItem;
            Node.Controller(mode, lengthMode, angleMode, point, node, isLeftControlPoint);
        }

        public override NodeRemoveMode NodeRemoveCheckedNodes() => NodeCollection.RemoveCheckedNodes(this.Nodes);
        public override void NodeInterpolationCheckedNodes() => NodeCollection.InterpolationCheckedNodes(this.Nodes);
        public override void NodeSharpCheckedNodes() => NodeCollection.SharpCheckedNodes(this.Nodes);
        public override void NodeSmoothCheckedNodes() => NodeCollection.SmoothCheckedNodes(this.Nodes);

    }
}