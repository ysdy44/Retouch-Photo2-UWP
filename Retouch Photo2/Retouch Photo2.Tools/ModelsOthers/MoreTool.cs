using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s MoreTool.
    /// </summary>
    public class MoreTool : ITool
    {
        //@Content
        public ToolType Type => ToolType.More;
        public string Title { get; set; }
        public FrameworkElement Icon => null;
        public bool IsSelected { get; set; }

        public FrameworkElement Button => new ToolMoreButton();
        public FrameworkElement Page => null;

        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) { }
        public void Clicke(Vector2 point) { }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }
}