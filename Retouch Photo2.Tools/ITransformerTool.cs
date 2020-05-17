﻿using FanKit.Transformers;
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
        /// <param name="disableTranslation"> Is disable <see cref="TransformerMode.Translation"/>?. </param>
        bool Started(Vector2 startingPoint, Vector2 point, bool disableTranslation);
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
        /// Select a layer from a point, make it to selection layer and make the TransformerMode to move.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        bool Clicke(Vector2 point);

        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        void Draw(CanvasDrawingSession drawingSession);
    }
}