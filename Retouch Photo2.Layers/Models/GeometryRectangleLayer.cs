using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRectangle;

        //@Construct
        /// <summary>
        /// Initializes a rectangle-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryRectangleLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        

        public override  ILayer Clone(CanvasDevice customDevice)
        {
            GeometryRectangleLayer rectangleLayer = new GeometryRectangleLayer(customDevice);

            LayerBase.CopyWith(customDevice, rectangleLayer, this);
            return rectangleLayer;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, matrix);
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            return new NodeCollection(geometry);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryRectangle");
        }

    }
}