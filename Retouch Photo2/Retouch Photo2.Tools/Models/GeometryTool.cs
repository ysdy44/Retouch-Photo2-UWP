using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    public class GeometryTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;


        public GeometryTool()
        {
            base.Type = ToolType.Geometry;
            base.Icon = new GeometryControl();
            base.WorkIcon = new GeometryControl();
            base.Page = new GeometryPage();
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
