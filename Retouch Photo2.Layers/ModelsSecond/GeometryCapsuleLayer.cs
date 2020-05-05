﻿using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryCapsuleLayer .
    /// </summary>
    public class GeometryCapsuleLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCapsule;

        //@Construct
        /// <summary>
        /// Construct a capsule-layer.
        /// </summary>
        public GeometryCapsuleLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCapsuleIcon(),
                Text = this.ConstructStrings(),
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            return TransformerGeometry.CreateCapsule
            (
                resourceCreator,
                transformer,
                canvasToVirtualMatrix
            );
        }

        
        public IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromCapsule(transformer);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCapsuleLayer CapsuleLayer = new GeometryCapsuleLayer();

            LayerBase.CopyWith(resourceCreator, CapsuleLayer, this);
            return CapsuleLayer;
        }
        
        public void SaveWith(XElement element) { }
        public void Load(XElement element) { }

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryCapsule");
        }

    }
}