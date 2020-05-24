using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s CurveMultiLayer .
    /// </summary>
    public class CurveMultiLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.CurveMulti;

        //@Content 
        public IList<NodeCollection> Nodess { get; private set; }

        //@Construct
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>
        public CurveMultiLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new CurveMultiIcon(),
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>
        /// <param name="nodess"> The source nodes. </param>
        public CurveMultiLayer(IList<NodeCollection> nodess) : this() => this.Nodess = nodess;
        /// <summary>
        /// Initializes a multi-curve-layer.
        /// </summary>
        /// <param name="nodess"> The source nodes. </param>
        public CurveMultiLayer(IEnumerable<IEnumerable<Node>> nodess) : this()
        {
            this.Nodess =
            (
                from nodes
                in nodess
                select new NodeCollection(nodes)
            ).ToList();
        }


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //TODO: GeometryCurveMultiLayer
            //   if (this.IsRefactoringTransformer)
            //  {
            //      Transformer transformer = LayerCollection.RefactoringTransformer(this.Nodes);
            //        this.Transform.Source = transformer;
            //       this.Transform.Destination = transformer;
            //
            //       this.IsRefactoringTransformer = false;
            //   }


            return this.Transform.IsCrop ? this.Transform.CropDestination : this.Transform.Destination;
        }

        public override void CacheTransform()
        {
            base.CacheTransform();
            foreach (NodeCollection ndoes in this.Nodess)
            {
                ndoes.CacheTransform();
            }
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);
            foreach (NodeCollection ndoes in this.Nodess)
            {
                ndoes.TransformMultiplies(matrix);
            }
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);
            foreach (NodeCollection ndoes in this.Nodess)
            {
                ndoes.TransformAdd(vector);
            }
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            CurveMultiLayer curveMultiLayer = new CurveMultiLayer
            {
                Nodess = (from nodes in this.Nodess select nodes.Clone()).ToList()
            };

            Layer.CopyWith(resourceCreator, curveMultiLayer, this);
            return curveMultiLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement
            (
                "Nodess",
                from nodes
                in this.Nodess
                select FanKit.Transformers.XML.SaveNodeCollection("Nodes", "Node", nodes)
            ));
        }
        public override void Load(XElement element)
        {
            XElement nodess = element.Element("Nodess");

            this.Nodess =
            (
                from nodes
                in nodess.Elements()
                select FanKit.Transformers.XML.LoadNodeCollection("Node", nodes.Element("Nodes"))
           ).ToList();
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            IEnumerable<CanvasGeometry> geometrys =
                 from nodes
                 in this.Nodess
                 select nodes.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);

            return CanvasGeometry.CreateGroup(resourceCreator, geometrys.ToArray());
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/CurveMulti");
        }

    }
}