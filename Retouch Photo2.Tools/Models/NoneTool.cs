// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★★
// Complete:      
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
        //@Content
        public ToolType Type => ToolType.None;
        public ToolGroupType GroupType => ToolGroupType.None;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page => null;
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) { }
        public void Clicke(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}