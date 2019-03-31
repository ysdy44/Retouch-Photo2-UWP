using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using System.Numerics;

namespace Retouch_Photo.Tools.Models
{
    public class PaintBrushTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new PaintBrushControl();
            base.WorkIcon = new PaintBrushControl();
            base.Page = new PaintBrushPage();
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
