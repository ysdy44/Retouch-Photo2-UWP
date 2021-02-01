// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★★
// Complete:      
using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can be operated. Method has bool type return value.
    /// </summary>
    public interface ITransformerTool
    {

        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        bool Started(Vector2 startingPoint, Vector2 point);
        /// <summary>
        /// Occurs when the input device changes position during operation.
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        bool Delta(Vector2 startingPoint, Vector2 point);
        /// <summary>
        /// Occurs when the operation completes.
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        bool Complete(Vector2 startingPoint, Vector2 point);

        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        void Draw(CanvasDrawingSession drawingSession);
    }

}