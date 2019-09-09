using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s NoneTool.
    /// </summary>
    public class NoneTool : ITool
    {
        public bool IsOpen { set { } }
        public ToolType Type => ToolType.None;
        public FrameworkElement Icon { get; } = null;
        public FrameworkElement ShowIcon { get; } = null;
        public Page Page { get; } = null;


        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) { }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}