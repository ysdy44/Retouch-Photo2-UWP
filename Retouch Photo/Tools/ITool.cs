using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo.Tools
{
    public abstract class ITool
    {
        //Operator
        public abstract bool Start(Vector2 point);
        public abstract bool Delta(Vector2 point);
        public abstract bool Complete(Vector2 point);

        public abstract bool Draw(CanvasDrawingSession ds);
    }
}
