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
        public RectangleLayer()
        {
            base.Control.Icon = new RectangleIcon();
            base.Control.Text = "Rectangle";
        }

        //@Override      
        public override string Type => "Rectangle";
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.ActualDestinationAboutGroupLayer;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            RectangleLayer rectangleLayer = new RectangleLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }
    }
}