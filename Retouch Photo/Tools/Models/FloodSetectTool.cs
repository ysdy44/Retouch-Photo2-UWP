using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Tools.Models
{
    public class FloodSetectTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        public FloodSetectTool()
        {
            base.Type = ToolType.FloodSetect;
            base.Icon = new FloodSetectControl();
            base.WorkIcon = new FloodSetectControl();
            base.Page = new FloodSetectPage();
        }


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
