using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Interface of <see cref = "TransformerTool" />.
    /// </summary>
    public abstract class ITransformerTool
    {
        /// <summary>
        /// Select a layer from a point,
        /// make it to <see cref = "Selection.Layer" />
        /// and make the <see cref = "TransformerMode" /> to move,
        /// find the layer that makes it unique, and 
        /// </summary>
        /// <param name="point"> point </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        public abstract bool SelectLayer(Vector2 point);


        /// <summary> <see cref = "Tool.Starting" />'s method. </summary>
        public abstract bool Starting(Vector2 point);

        /// <summary> <see cref = "Tool.Started" />'s method. </summary>
        public abstract bool Started(Vector2 startingPoint, bool isSetTransformerMode = true);

        /// <summary> <see cref = "Tool.Delta" />'s method. </summary>
        public abstract bool Delta(Vector2 startingPoint, Vector2 point);

        /// <summary> <see cref = "Tool.Complete" />'s method. </summary>
        public abstract bool Complete(bool isSingleStarted);
     
        /// <summary> <see cref = "Tool.Draw" />'s method. </summary>
        public abstract void Draw(CanvasDrawingSession ds);
    }
}