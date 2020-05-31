using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s CurveMultiLayer .
    /// </summary>
    public partial class CurveMultiLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.CurveMulti;

        //@Content 
        public NodeCollectionCollection Nodess { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>   
        /// <param name="customDevice"> The custom-device. </param>
        public CurveMultiLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="nodess"> The source nodes. </param>
        public CurveMultiLayer(CanvasDevice customDevice, IList<NodeCollection> nodess) : this(customDevice) => this.Nodess = new NodeCollectionCollection(nodess);
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="nodess"> The source nodes. </param>
        public CurveMultiLayer(CanvasDevice customDevice, IEnumerable<IEnumerable<Node>> nodess) : this(customDevice) => this.Nodess = new NodeCollectionCollection(nodess);


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                TransformerBorder border = new TransformerBorder(this.Nodess);
                Transformer transformer = border.ToTransformer();
                this.Transform.Transformer = transformer;
                return this.Transform.IsCrop ? this.Transform.CropTransformer : transformer;
            }

            return this.Transform.IsCrop ? this.Transform.CropTransformer : this.Transform.Transformer;
        }

        public override void CacheTransform()
        {
            base.CacheTransform();
            this.Nodess.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
            this.Nodess.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
            this.Nodess.TransformAdd(vector);
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            CurveMultiLayer curveMultiLayer = new CurveMultiLayer(customDevice)
            {
                Nodess = this.Nodess.Clone()
            };

            LayerBase.CopyWith(customDevice, curveMultiLayer, this);
            return curveMultiLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveNodeCollectionCollection("Nodess", this.Nodess));
        }
        public override void Load(XElement element)
        {
            this.Nodess = FanKit.Transformers.XML.LoadNodeCollectionCollection(element.Element("Nodess"));
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            return this.Nodess.CreateGeometry(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return this.Nodess.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);
        }
    }

    /// <summary>
    /// <see cref="LayerBase"/>'s CurveMultiLayer .
    /// </summary>
    public partial class CurveMultiLayer : LayerBase, ILayer
    {

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/CurveMulti");
        }

        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;

        public override void DrawNode(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            foreach (NodeCollection ndoes in this.Nodess)
            {
                drawingSession.DrawNodeCollection(ndoes, matrix, accentColor);
            }
        }

        public override NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, Matrix3x2 matrix)
        {
            return NodeCollectionCollection.ContainsNodeCollectionMode(point, this.Nodess, matrix);
        }

        public override void NodeCacheTransform() => this.Nodess.CacheTransform();
        public override void NodeTransformMultiplies(Matrix3x2 matrix) => this.Nodess.TransformMultipliesOnlySelected(matrix);
        public override void NodeTransformAdd(Vector2 vector) => this.Nodess.TransformAddOnlySelected(vector);
        
        public override bool NodeSelectionOnlyOne(Vector2 point, Matrix3x2 matrix) => this.Nodess.SelectionOnlyOne(point, matrix);

        public override void NodeBoxChoose(TransformerRect boxRect) => this.Nodess.BoxChoose(boxRect);

        public override void NodeMovePoint(Vector2 point)
        {
            NodeCollection nodes = this.Nodess.SelectedItem;
            Node node = nodes.SelectedItem;
            Node.Move(point, node);
        }
        public override void NodeControllerControlPoint(SelfControlPointMode mode, EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 point, bool isLeftControlPoint)
        {
            NodeCollection nodes = this.Nodess.SelectedItem;
            Node node = nodes.SelectedItem;
            Node.Controller(mode, lengthMode, angleMode, point, node, isLeftControlPoint);
        }

        public override NodeRemoveMode NodeRemoveCheckedNodes() => NodeCollectionCollection.RemoveCheckedNodes(this.Nodess);
        public override void NodeInterpolationCheckedNodes() => NodeCollectionCollection.InterpolationCheckedNodes(this.Nodess);
        public override void NodeSharpCheckedNodes() => NodeCollectionCollection.SharpCheckedNodes(this.Nodess);
        public override void NodeSmoothCheckedNodes() => NodeCollectionCollection.SmoothCheckedNodes(this.Nodess);

    }
}