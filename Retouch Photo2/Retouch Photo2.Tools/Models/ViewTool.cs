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
    /// <see cref="ITool"/>'s ViewTool.
    /// </summary>
    public class ViewTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        public ToolType Type => ToolType.View;
        public FrameworkElement Icon { get; } = new ViewControl();
        public FrameworkElement ShowIcon { get; } = new ViewControl();
        public Page Page => this._viewPage;
        ViewPage _viewPage { get; } = new ViewPage();

        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            this.ViewModel.CanvasTransformer.CacheMove(startingPoint);
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (isSingleStarted) this.ViewModel.CanvasTransformer.Move(point);
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public void Draw(CanvasDrawingSession drawingSession) { }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}