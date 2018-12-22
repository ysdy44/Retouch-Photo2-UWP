using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.ViewModels
{
    public abstract class ToolViewModel
    {
        public abstract void Start(Vector2 point);
        public abstract void Delta(Vector2 point);
        public abstract void Complete(Vector2 point);

        public abstract void Draw(CanvasDrawingSession ds);
    }
    public abstract class ToolViewModel2
    {
        public abstract void Start(Vector2 point, Layer layer);
        public abstract void Delta(Vector2 point, Layer layer);
        public abstract void Complete(Vector2 point, Layer layer);

        public abstract void Draw(CanvasDrawingSession ds, Layer layer);
    }
}
