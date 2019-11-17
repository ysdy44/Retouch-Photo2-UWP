﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCurveMultiLayer .
    /// </summary>
    public class GeometryCurveMultiLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCurveMulti;

        //@Content 
        public IList<NodeCollection> Nodess { get; private set; }

        //@Construct
        /// <summary>
        /// Construct a multi-curve-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryCurveMultiLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a multi-curve-layer.
        /// </summary>
        private GeometryCurveMultiLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCurveMultiIcon(),
                Text = "CurveMulti",
            };
        }
    
        /// <summary>
        /// Construct a multi-curve-layer.
        /// </summary>
        /// <param name="nodess"> The source nodes. </param>
        public GeometryCurveMultiLayer(IList<NodeCollection> nodess) : this() => this.Nodess = nodess;
        /// <summary>
        /// Construct a multi-curve-layer.
        /// </summary>
        /// <param name="nodess"> The source nodes. </param>
        public GeometryCurveMultiLayer(IEnumerable<IEnumerable<Node>> nodess) : this()
        {
            this.Nodess =
            (
                from nodes
                in nodess
                select new NodeCollection(nodes)
            ).ToList();                            
        }


        //@Override
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


        public override Transformer GetActualDestinationWithRefactoringTransformer
        {
            get
            {
                //TODO: GeometryCurveMultiLayer
                //   if (this.IsRefactoringTransformer)
                //  {
                //      Transformer transformer = LayerCollection.RefactoringTransformer(this.Nodes);
                //        this.TransformManager.Source = transformer;
                //       this.TransformManager.Destination = transformer;
                //
                //       this.IsRefactoringTransformer = false;
                //   }

                return base.GetActualDestinationWithRefactoringTransformer;
            }
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            IEnumerable<CanvasGeometry> geometrys =
                 from nodes
                 in this.Nodess
                 select nodes.CreateGeometry(resourceCreator).Transform(canvasToVirtualMatrix);

            return CanvasGeometry.CreateGroup(resourceCreator, geometrys.ToArray());
        }


        public IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCurveMultiLayer CurveMultiLayer = new GeometryCurveMultiLayer
            {
                Nodess = (from nodes in this.Nodess select nodes.Clone()).ToList()
            };

            LayerBase.CopyWith(resourceCreator, CurveMultiLayer, this);
            return CurveMultiLayer;
        }

        public void SaveWith(XElement element)
        {
            element.Add(new XElement
            (
                "Nodess",
                from nodes
                in this.Nodess
                select FanKit.Transformers.XML.SaveNodeCollection("Nodes", "Node", nodes)
            ));
        }
        public void Load(XElement element)
        {
            XElement nodess = element.Element("Nodess");

            this.Nodess =
            (
                from nodes
                in nodess.Elements()
                select FanKit.Transformers.XML.LoadNodeCollection("Node", nodes.Element("Nodes"))
           ).ToList();
        }

    }
}