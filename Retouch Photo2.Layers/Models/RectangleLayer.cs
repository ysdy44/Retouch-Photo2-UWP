using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s RectangleLayer .
    /// </summary>
    public class RectangleLayer : IGeometryLayer
    {
        //@Construct
        public RectangleLayer(LayerCollection layerCollection) : base(layerCollection)
        {
            base.Control.Icon = new RectangleIcon();
        }

        //@Override      
        public override string Type => "Rectangle";
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public override ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator)
        {
            RectangleLayer rectangleLayer = new RectangleLayer(layerCollection)
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(layerCollection, resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }
    }
}