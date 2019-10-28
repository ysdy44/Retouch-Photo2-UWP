using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

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
                Text = "Capsule",
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


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCapsuleLayer CapsuleLayer = new GeometryCapsuleLayer();

            LayerBase.CopyWith(resourceCreator, CapsuleLayer, this);
            return CapsuleLayer;
        }


        public void SaveWith(XElement element) { }

    }
}