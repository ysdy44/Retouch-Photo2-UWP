using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public class PenTool : ITool
    {
        //@PenModel
        ViewModel PenModel => App.ViewModel;


        public ToolType Type=> ToolType.Pen;
        public FrameworkElement Icon { get; }= new PenControl();
        public FrameworkElement ShowIcon { get; } = new PenControl();
        public Page Page { get; }= new PenPage();
        
        
        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)        {        }
        public void Delta(Vector2 startingPoint, Vector2 point)        {        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)        {        }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}