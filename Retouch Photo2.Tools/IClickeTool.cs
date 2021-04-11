// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★★
// Complete:      
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can be clicked. Method has bool type return value.
    /// </summary>
    public interface IClickeTool
    {

        /// <summary>
        /// Select a layer from a point, make it to selection layer and make the TransformerMode to move.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        bool Clicke(Vector2 point);

        /// <summary>
        /// Occurs when the cursor pointer is moved.
        /// </summary>
        void Cursor(Vector2 point);

    }
}