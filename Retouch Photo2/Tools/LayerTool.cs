using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using Retouch_Photo2.Tools.ITools;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools
{
    public abstract class LayerTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //@override
        public abstract Layer GetLayer(VectRect rect);

        readonly ICursorTool ICursorTool = new ICursorTool();
        readonly IClickedTool IClickedTool;
        readonly ICreateTool ICreateTool;

        public LayerTool()
        {
            this.ICreateTool = new ICreateTool(this.GetLayer);
            this.IClickedTool = new IClickedTool(this.ICreateTool.Start, this.ICreateTool.Delta, this.ICreateTool.Complete);
        }

        public override void Start(Vector2 point)
        {
            if (this.ICursorTool.Start(point)) return;

            this.IClickedTool.Start(point);
        }
        public override void Delta(Vector2 point)
        {
            if (this.ICursorTool.Delta(point)) return;

            this.IClickedTool.Delta(point);
        }
        public override void Complete(Vector2 point)
        {
            if (this.ICursorTool.Complete(point)) return;

            this.IClickedTool.Complete(point);
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            this.ICreateTool.Draw(ds); 
            this.ICursorTool.Draw(ds); 
        }
    }
}