using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Represents a tool that can be operated. Provides OnNavigated to, form methods
    /// </summary>
    public abstract class Tool : ITool
    {
        public ToolType Type { get; set; }
        public FrameworkElement Icon { get; set; }
        public FrameworkElement ShowIcon { get; set; }
        public Page Page { get; set; }


        //@Abstract
        public abstract void Starting(Vector2 point);
        public abstract void Started(Vector2 startingPoint, Vector2 point);
        public abstract void Delta(Vector2 startingPoint, Vector2 point);
        public abstract void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted);

        public abstract void Draw(CanvasDrawingSession drawingSession);


        //@Virtual
        public virtual void OnNavigatedTo() { }
        public virtual void OnNavigatedFrom() { }
    }
}