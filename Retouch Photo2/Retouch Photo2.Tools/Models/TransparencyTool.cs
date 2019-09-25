using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s TransparencyTool.
    /// </summary>
    public class TransparencyTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        public bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._transparencyPage.IsSelected = value;
            }
        }
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon { get; } = new TransparencyIcon();
        public IToolButton Button { get; } = new TransparencyButton();
        public Page Page => this._transparencyPage;
        TransparencyPage _transparencyPage { get; } = new TransparencyPage();

        public void Starting(Vector2 point)
        {
        }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {

        }

        public void Draw(CanvasDrawingSession drawingSession)
        {

        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}