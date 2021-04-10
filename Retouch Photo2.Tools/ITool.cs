// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         
// Only:              ★★★★
// Complete:      
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can be operated.
    /// </summary>
    public interface ITool
    {
        /// <summary> Gets the type. </summary>
        ToolType Type { get; } 

        /// <summary> Gets the icon. </summary>
        ControlTemplate Icon { get; }
        /// <summary> Gets the page. </summary>
        FrameworkElement Page { get; }

        /// <summary> Gets or sets the IsSelected. </summary>
        bool IsSelected { get; set; }
        /// <summary> Gets or sets the IsOpen. </summary>
        bool IsOpen { set; }


        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        void Started(Vector2 startingPoint, Vector2 point);
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
        /// Occurs when the canvas is clicked.
        /// </summary>
        void Clicke(Vector2 point);

        /// <summary>
        ///Occurs when the pointer is moved.
        /// </summary>
        void Cursor(Vector2 point);

        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        void Draw(CanvasDrawingSession drawingSession);


        /// <summary> The current tool becomes the active tool. </summary>
        void OnNavigatedTo();
        /// <summary> The current page does not become an active page. </summary>
        void OnNavigatedFrom();
    }
}