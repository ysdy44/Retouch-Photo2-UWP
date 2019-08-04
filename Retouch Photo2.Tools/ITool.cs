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
        /// <summary> Gets or Sets IITool's type. </summary>
        ToolType Type { get; set; }
        /// <summary> Gets or Sets IITool's icon. </summary>
        FrameworkElement Icon { get; set; }
        /// <summary> Gets or Sets IITool's icon2. </summary>
        FrameworkElement ShowIcon { get; set; }
        /// <summary> Gets or Sets IITool's page. </summary>
        Page Page { get; set; }


        /// <summary>
        /// Occurs the first time an action processor is created.
        /// </summary>
        /// <param name="point"> The pointer. </param>
        void Starting(Vector2 point);
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
        /// <param name="isSingleStarted"> Whether the Started method was triggered. </param>
        void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted);

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