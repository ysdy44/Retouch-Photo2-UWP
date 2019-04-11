using Microsoft.Graphics.Canvas;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Models.CursorTools;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;

namespace Retouch_Photo.Tools.Models
{
    public class CursorTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        readonly ICursorTool ICursorTool = new ICursorTool();
        readonly ITranslation ICursor = new ITranslation();
        readonly IBlueBox IBlueBox = new IBlueBox();

        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.WorkIcon = new CursorControl();
            base.Page = new CursorPage();
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
            // Transformer
            if (this.ICursorTool.Start(point)) return;

            if (this.ICursor.Start(point)) return;
            if (this.IBlueBox.Start(point)) return;
        }
        public override void Delta(Vector2 point)
        {
            // Transformer
            if (this.ICursorTool.Delta(point)) return;

            if (this.ICursor.Delta(point)) return;
            if (this.IBlueBox.Delta(point)) return;
        }
        public override void Complete(Vector2 point)
        {
            // Transformer
            if (this.ICursorTool.Complete(point)) return;

            if (this.ICursor.Complete(point)) return;
            if (this.IBlueBox.Complete(point)) return;
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.IBlueBox.Draw(ds)) return;
            
            // Transformer
            if (this.ICursorTool.Draw(ds)) return;

            //Cursor
            if (this.ICursor.Draw(ds)) return;

        }
    }
}
