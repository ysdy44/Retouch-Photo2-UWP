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
        public CurveLayer(CanvasDevice customDevice, IEnumerable<Node> nodes) : this(customDevice) => base.Nodes = new NodeCollection(nodes);
        /// <summary>
        /// Initializes a curve-layer.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="nodes"> The source nodes. </param>
        public CurveLayer(CanvasDevice customDevice, NodeCollection nodes) : this(customDevice) => base.Nodes = nodes;
        /// <summary>
        /// Initializes a curve-layer from a line.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="left"> The first source vector. </param>
        /// <param name="right"> The second source vector. </param>
        public CurveLayer(CanvasDevice customDevice, Vector2 left, Vector2 right) : this(customDevice) => base.Nodes = new NodeCollection(left, right);


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                TransformerBorder border = new TransformerBorder(base.Nodes);
                Transformer transformer = border.ToTransformer();
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


        public override ILayer Clone(CanvasDevice customDevice)
        {
            CurveLayer curveLayer = new CurveLayer(customDevice)
            {
                Nodes = base.Nodes.Clone()
            };

            LayerBase.CopyWith(customDevice, curveLayer, this);
            return curveLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(FanKit.Transformers.XML.SaveNodeCollection("Nodes", base.Nodes));
        }
        public override void Load(XElement element)
        {
            base.Nodes = FanKit.Transformers.XML.LoadNodeCollection(element.Element("Nodes"));
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
        
        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Curve");
        }
    }
}