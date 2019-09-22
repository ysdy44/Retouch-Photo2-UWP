using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public class CropTool : ITool
    {
        public bool IsSelected { set { } }
        public ToolType Type => ToolType.Crop;
        public FrameworkElement Icon { get; } = new CropIcon();
        public IToolButton Button { get; } = new CropButton();
        public Page Page { get; } = new CropPage();


        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point) { }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) { }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}