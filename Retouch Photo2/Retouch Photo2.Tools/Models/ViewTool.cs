using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s ViewTool.
    /// </summary>
    public class ViewTool : Tool
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        Vector2 StartPosition;

        //@Construct
        public ViewTool()
        {
            base.Type = ToolType.View;
            base.Icon = new ViewControl();
            base.ShowIcon = new ViewControl();
            base.Page = new ViewPage();
        }

        //@Override
        public override void Starting(Vector2 point) { }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            this.StartPosition = this.ViewModel.CanvasTransformer.Position;

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            this.ViewModel.CanvasTransformer.Position = this.StartPosition - startingPoint + point;
            this.ViewModel.CanvasTransformer.ReloadMatrix();

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (isSingleStarted)
            {
                this.ViewModel.CanvasTransformer.Position = this.StartPosition - startingPoint + point;
                this.ViewModel.CanvasTransformer.ReloadMatrix();
            }
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        } 

        public override void Draw(CanvasDrawingSession ds) { }
    }
}