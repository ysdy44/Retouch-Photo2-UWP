using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s IGeometryLayer .
    /// </summary>
    public abstract class IGeometryLayer : LayerBase
    {
        //@Abstract
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <returns> The product geometry. </returns>
        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);
                

        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.CreateGeometry(resourceCreator, canvasToVirtualMatrix);
                //Fill
                this.StyleManager.FillGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
                //Stroke
                this.StyleManager.DrawGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
            }
            return command;
        }
        public override void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator, matrix);
            drawingSession.DrawGeometry(geometry, accentColor);
        }      
    }
}