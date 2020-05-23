using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GeometryCapsuleLayer .
    /// </summary>
    public class GeometryCapsuleLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryCapsule;

        //@Construct
        /// <summary>
        /// Initializes a capsule-layer.
        /// </summary>
        public GeometryCapsuleLayer()
        {
            base.Control = new LayerControl
            {
                Icon = new GeometryCapsuleIcon(),
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCapsuleLayer capsuleLayer = new GeometryCapsuleLayer();

            Layer.CopyWith(resourceCreator, capsuleLayer, this);
            return capsuleLayer;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreateCapsule(resourceCreator, transformer, canvasToVirtualMatrix);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromCapsule(transformer);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryCapsule");
        }

    }
}