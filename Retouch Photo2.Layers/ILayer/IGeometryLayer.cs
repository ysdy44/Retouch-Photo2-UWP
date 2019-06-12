using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;

namespace Retouch_Photo2.Layers.ILayer
{
    /// <summary>
    /// <see cref="Layer"/>'s IGeometryLayer .
    /// </summary>
    public abstract class IGeometryLayer:Layer
    {
        //@Abstract
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> geometry </returns>
        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);

        //@Override
        public override Color? GetFillColor() => this.FillColor;
        public override void SetFillColor(Color fillColor) => this.FillColor = fillColor;


        /// <summary> <see cref = "IGeometryLayer" />'s fill-color. </summary>
        public Color FillColor = Colors.Gray;

  
        //@Override
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.CreateGeometry(resourceCreator, canvasToVirtualMatrix);
                ds.FillGeometry(geometry, this.FillColor);
            }
            return command;
        }
    }
}