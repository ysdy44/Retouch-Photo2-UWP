// Core:              ★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryEllipseLayer .
    /// </summary>
    public class GeometryEllipseLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryEllipse;

        //@Construct
        /// <summary>
        /// Initializes a ellipse-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryEllipseLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }               


        public override  ILayer Clone(CanvasDevice customDevice)
        {
            GeometryEllipseLayer ellipseLayer = new GeometryEllipseLayer(customDevice);

            LayerBase.CopyWith(customDevice, ellipseLayer, this);
            return ellipseLayer;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToEllipse(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToEllipse(resourceCreator, matrix);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_GeometryEllipse");
        }

    }
}