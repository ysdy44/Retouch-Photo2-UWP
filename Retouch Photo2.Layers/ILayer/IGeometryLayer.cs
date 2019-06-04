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
        /// Get a specific geometry.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> geometry </returns>
        public abstract CanvasGeometry GetGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);

        public Color Color = Colors.Gray;

        //@Override
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.GetGeometry(resourceCreator, canvasToVirtualMatrix);
                ds.FillGeometry(geometry, this.Color);
            }
            return command;
        }
    }
}