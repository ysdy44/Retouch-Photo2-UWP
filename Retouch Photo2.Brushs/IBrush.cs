using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that provides an <see cref="ICanvasBrush"/>.
    /// </summary>
    public interface IBrush: ICacheTransform
    {

        /// <summary> Gets of sets the type. </summary>
        BrushType Type { get; set; }

        /// <summary> Gets of sets the color. </summary>
        Color Color { get; set; }

        /// <summary> Gets of sets the stops. </summary>
        CanvasGradientStop[] Stops { get; set; }

        /// <summary> Gets of sets the photocopier. </summary>
        Photocopier Photocopier { get; set; }
        /// <summary> Gets of sets the extend. </summary>
        CanvasEdgeBehavior Extend { get; set; }

        /// <summary> Gets of sets the center point. </summary>
        Vector2 Center { get; set; }
        /// <summary> Gets of sets the x-point. </summary>
        Vector2 XPoint { get; set; }
        /// <summary> Gets of sets the y-point. </summary>
        Vector2 YPoint { get; set; }

        
        /// <summary>
        /// Gets <see cref="ICanvasBrush"/>.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided <see cref="ICanvasBrush"/>. </returns>
        ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix);


        /// <summary>
        /// Gets the all points by the brush contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The operate-mode. </returns>
        BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix);
        /// <summary>
        /// It controls the transformation of brush.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        void Controller(BrushOperateMode mode, Vector2 startingPoint, Vector2 point);
        /// <summary>
        /// It initialize and controls the transformation of brush.
        /// </summary>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        void InitializeController(Vector2 startingPoint, Vector2 point);
        
        /// <summary>
        /// Draw stops and lines between all control points.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor);


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned <see cref="IBrush"/>. </returns>
        IBrush Clone();

    }
}