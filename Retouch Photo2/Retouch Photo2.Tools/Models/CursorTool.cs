using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.ITools;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    public class CursorTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        readonly IClickedTool IClickedTool;

        readonly ICursorTool ICursorTool = new ICursorTool();

        readonly ITranslation ITranslation = new ITranslation();
        readonly IBlueBox IBlueBox = new IBlueBox();

        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.WorkIcon = new CursorControl();
            base.Page = new CursorPage();

            this.IClickedTool = new IClickedTool
            (
               start: (point) =>
               {
                   // Transformer
                   if (this.ICursorTool.Start(point)) return true;

                   if (this.ITranslation.Start(point)) return true;
                   if (this.IBlueBox.Start(point)) return true;
                   return true;
               },
               delta: (point) =>
               {
                   // Transformer
                   if (this.ICursorTool.Delta(point)) return true;

                   if (this.ITranslation.Delta(point)) return true;
                   if (this.IBlueBox.Delta(point)) return true;
                   return true;
               },
               complete: (point) =>
               {
                   // Transformer
                   if (this.ICursorTool.Complete(point)) return true;

                   if (this.ITranslation.Complete(point)) return true;
                   if (this.IBlueBox.Complete(point)) return true;
                   return true;
               }
            );
        }
        

        public override void Start(Vector2 point)=> this.IClickedTool.Start(point);
        public override void Delta(Vector2 point) => this.IClickedTool.Delta(point);
        public override void Complete(Vector2 point)
        {
            this.IClickedTool.Complete(point);
            this.ViewModel.Invalidate();
        }
                 

        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.IBlueBox.Draw(ds)) return;

            // Transformer
            if (this.ICursorTool.Draw(ds)) return;

            //Cursor
            if (this.ITranslation.Draw(ds)) return;
        }
    }
}
