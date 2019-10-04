using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s EllipseLayer .
    /// </summary>
    public class EllipseLayer : IGeometryLayer
    {
        //@Construct
        public EllipseLayer(LayerCollection layerCollection) : base(layerCollection)
        {
            base.Control.Icon = new EllipseIcon();
        }

        //@Override       
        public override string Type => "Ellipse";
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator)
        {
            EllipseLayer ellipseLayer= new EllipseLayer(layerCollection)
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(layerCollection, resourceCreator, ellipseLayer, this);
            return ellipseLayer;
        }
    }
}