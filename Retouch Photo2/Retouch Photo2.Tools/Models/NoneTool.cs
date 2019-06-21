using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s NoneTool.
    /// </summary>
    public class NoneTool : Tool
    {
        //@Construct
        public NoneTool()
        {
            base.Type = ToolType.None;
            base.Icon = null;
            base.ShowIcon = null;
            base.Page = null;
        }

        //@Override
        public override void Starting(Vector2 point) { }
        public override void Started(Vector2 startingPoint, Vector2 point) { }
        public override void Delta(Vector2 startingPoint, Vector2 point) { }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) { }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}