using Microsoft.Graphics.Canvas;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;

namespace Retouch_Photo.Tools.Models
{
    public class WatercolorPenTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        public WatercolorPenTool()
        {
            base.Type = ToolType.WatercolorPen;
            base.Icon = new WatercolorPenControl();
            base.WorkIcon = new WatercolorPenControl();
            base.Page = new WatercolorPenPage();
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
