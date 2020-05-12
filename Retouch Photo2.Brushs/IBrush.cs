using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Elements;
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

        /// <summary> <see cref="IBrush">'s type. </summary>
        BrushType Type { get; }

        /// <summary> Gets <see cref="GradientBrush">'s array. </summary>
        CanvasGradientStop[] Array { get; set; }
        /// <summary> <see cref="ColorBrush"/>'s Color. </summary>
        Color Color { get; set; }
        /// <summary> <see cref="ImageBrush"/>'s Destination. </summary>
        Transformer Destination { set; }
        /// <summary> <see cref="ImageBrush"/>'s Photocopier. </summary>
        Photocopier Photocopier { get; }
        /// <summary> <see cref="ImageBrush"/>'s Extend. </summary>
        CanvasEdgeBehavior Extend { get; set; }


        /// <summary>
        /// Gets <see cref="ICanvasBrush"/>.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The provided <see cref="ICanvasBrush"/>. </returns>
        ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator);
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
        /// Draw stops and lines between all control points.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor);


        /// <summary>
        /// Get <see cref="IBrush"/> own copy.
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
        /// Points turn to _oldPoints in Transformer.One.
        /// </summary>
        /// <param name="transformer"> The Transformer about Points. </param>
        void OneBrushPoints(Transformer transformer);
        /// <summary>
        /// _oldPoints turn to Points in Transformer Destination.
        /// </summary>
        /// <param name="transformer"> The Transformer about _oldPoints. </param>
        void DeliverBrushPoints(Transformer transformer);

    }
}