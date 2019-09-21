using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using Retouch_Photo2.Layers.Models;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s RectangleLayer .
    /// </summary>
    public class RectangleLayer : IGeometryLayer
    {
        //@Override      
        public override string Type => "Rectangle";
        public override UIElement Icon=> new RectangleIcon();


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return base.Destination.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new RectangleLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };
        }
    }
}