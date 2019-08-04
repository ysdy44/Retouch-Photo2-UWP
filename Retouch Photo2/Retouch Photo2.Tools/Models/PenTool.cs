using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s PenTool.
    /// </summary>
    public class PenTool : Tool
    {
        //@PenModel
        ViewModel PenModel => App.ViewModel;


        //@Construct
        public PenTool()
        {
            base.Type = ToolType.Pen;
            base.Icon = new PenControl();
            base.ShowIcon = new PenControl();
            base.Page = new PenPage();
        }

        //@Override
        public override void Starting(Vector2 point) { }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {

        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {

        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
        }

        public override void Draw(CanvasDrawingSession drawingSession) { }
    }
}