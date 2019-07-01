using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Controls;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s BrushTool.
    /// </summary>
    public class BrushTool : Tool
    {
        //@Construct
        public BrushTool()
        {
            base.Type = ToolType.Brush;
            base.Icon = new BrushControl();
            base.ShowIcon = new BrushControl();
            base.Page = new BrushPage();
        }

        //@Override
        public override void Starting(Vector2 point)
        {
        }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}