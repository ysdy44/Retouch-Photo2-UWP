using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can be operated.
    /// </summary>
    public interface ITool
    {
        /// <summary> Gets IITool's type. </summary>
        ToolType Type { get; }
        /// <summary> Gets IITool's icon. </summary>
        FrameworkElement Icon { get; }
        /// <summary> Sets IITool's IsSelected. </summary>
        bool IsSelected { get; set; }

        /// <summary> Gets IITool's button. </summary>
        FrameworkElement Button { get; }
        /// <summary> Gets IITool's page. </summary>
        FrameworkElement Page { get; }

        
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
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        void Draw(CanvasDrawingSession drawingSession);


        /// <summary> The current tool becomes the active tool. </summary>
        void OnNavigatedTo();
        /// <summary> The current page does not become an active page. </summary>
        void OnNavigatedFrom();
    }
}