using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
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

        
        /// <summary> <see cref = "IGeometryLayer" />'s fill-brush. </summary>
        public Brush FillBrush = new Brush();
        /// <summary> <see cref = "IGeometryLayer" />'s stroke-brush. </summary>
        public Brush StrokeBrush = new Brush();


        //@Override
        public override Color? GetFillColor() => this.FillBrush.Color;
        public override void SetFillColor(Color fillColor) => this.FillBrush.Color = fillColor;
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.CreateGeometry(resourceCreator, canvasToVirtualMatrix);

                this.FillBrush.FillGeometry(resourceCreator, ds, geometry, canvasToVirtualMatrix);
                this.FillBrush.DrawGeometry(resourceCreator, ds, geometry, canvasToVirtualMatrix, 1);
            }
            return command;
        }
    }
}