using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class NullViewModel : ToolViewModel
    {


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        public override void Start(Vector2 point)
        {
        }
        public override void Delta(Vector2 point)
        {
        }
        public override void Complete(Vector2 point)
        {
        }
        
        public override void Draw(CanvasDrawingSession ds)
        {
        }
    }
}
