// Core:              ★★★★
// Referenced:   ★★★
// Difficult:         ★★★
// Only:              ★★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Photos;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that provides an <see cref="ICanvasBrush"/>.
    /// </summary>
    public interface IBrush: ICacheTransform
    {

        /// <summary> Gets the type. </summary>
        BrushType Type { get; }

        /// <summary> Gets of sets the color. </summary>
        Color Color { get; set; }

        /// <summary> Gets of sets the extend. </summary>
        CanvasEdgeBehavior Extend { get; set; }
        /// <summary> Gets of sets the stops. </summary>
        CanvasGradientStop[] Stops { get; set; }
        /// <summary> Gets of sets the photocopier. </summary>
        Photocopier Photocopier { get; set; }
        
        /// <summary> Gets of sets the center point. (LinearGradientBrush.StartPoint) (RadialGradientBrush.Center) </summary>
        Vector2 Center { get; set; }
        /// <summary> Cache of <see cref="IBrush.Center"/>. </summary>
        Vector2 StartingCenter { get; }
        /// <summary> Gets of sets the x-point. </summary>
        Vector2 XPoint { get; set; }
        /// <summary> Cache of <see cref="IBrush.XPoint"/>. </summary>
        Vector2 StartingXPoint { get; }
        /// <summary> Gets of sets the y-point. (LinearGradientBrush.EndPoint) (RadialGradientBrush.Point) </summary>
        Vector2 YPoint { get; set; }
        /// <summary> Cache of <see cref="IBrush.YPoint"/>. </summary>
        Vector2 StartingYPoint { get; }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned <see cref="IBrush"/>. </returns>
        IBrush Clone();

        /// <summary>
        /// Saves the entire <see cref="IBrush"/> to a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        void SaveWith(XElement element);
        /// <summary>
        /// Load the entire <see cref="IBrush"/> form a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        void Load(XElement element);


        /// <summary>
        /// Gets <see cref="ICanvasBrush"/>.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The provided <see cref="ICanvasBrush"/>. </returns>
        ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator);


        /// <summary>
        /// Gets the all points by the brush contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The operate-mode. </returns>
        BrushHandleMode ContainsHandleMode(Vector2 point, Matrix3x2 matrix);
        /// <summary>
        /// It controls the transformation of brush.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        void Controller(BrushHandleMode mode, Vector2 startingPoint, Vector2 point);
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
        /// Change the brush's type.
        /// </summary>
        /// <param name="type"> The new type. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photo"> The photo. </param>
        void TypeChanged(BrushType type, Transformer transformer, Photo photo);
        /// <summary>
        /// Change the brush's type.
        /// </summary>
        /// <param name="type"> The new type. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="color"> The color. </param>
        /// <param name="photo"> The photo. </param>
        void TypeChanged(BrushType type, Transformer transformer, Color color, Photo photo);
        

    }
}