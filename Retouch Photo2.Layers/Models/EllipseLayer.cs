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
        //@Override       
        public override string Type => "Ellipse";
        public override UIElement Icon => new EllipseIcon();
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            return base.Destination.ToEllipse(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new EllipseLayer
            {
                Name=base.Name,
                Opacity= base.Opacity,
                BlendType= base.BlendType,

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