using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using System;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can create layer.
    /// </summary>
    public interface ICreateTool
    {

        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="createLayer">
        /// <summary>
        /// Function of how to crate a layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        /// </param>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        void Started(Func<CanvasDevice , Transformer, ILayer> createLayer, Vector2 startingPoint, Vector2 point);
        /// <summary>
        /// Occurs when the input device changes position during operation.
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        void Delta(Vector2 startingPoint, Vector2 point);
        /// <summary>
        /// Occurs when the operation completes.
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        /// <param name="isOutNodeDistance"> Whether the distance'LengthSquared exceeds [NodeDistance].. </param>
        void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance);

        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        void Draw(CanvasDrawingSession drawingSession);

    }
}