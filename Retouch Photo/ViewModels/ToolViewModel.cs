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
        public abstract void Start(Vector2 point, DrawViewModel viewModel);
        public abstract void Delta(Vector2 point, DrawViewModel viewModel);
        public abstract void Complete(Vector2 point, DrawViewModel viewModel);

        public abstract void Draw(CanvasDrawingSession ds, DrawViewModel viewModel);
    }
    public abstract class ToolViewModel2
    {
        public abstract void Start(Vector2 point, Layer layer, DrawViewModel viewModel);
        public abstract void Delta(Vector2 point, Layer layer, DrawViewModel viewModel);
        public abstract void Complete(Vector2 point, Layer layer, DrawViewModel viewModel);

        public abstract void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel);
    }
}
