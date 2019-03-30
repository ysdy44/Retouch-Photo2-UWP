using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using System.Numerics;

namespace Retouch_Photo.Tools.ViewModels
{
    public class ViewViewModel : ToolViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        Vector2 rightStartPoint;
        Vector2 rightStartPosition;


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        public override void Start(Vector2 point)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = this.ViewModel.MatrixTransformer.Position;

            this.ViewModel.Invalidate(isThumbnail: true);
        }
        public override void Delta(Vector2 point)
        {
            this.ViewModel.MatrixTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            this.ViewModel.Invalidate();
        }
        public override void Complete(Vector2 point)
        {
            this.ViewModel.Invalidate(isThumbnail: false);
        }
        
        public override void Draw(CanvasDrawingSession ds)
        {
        }
    }
}


