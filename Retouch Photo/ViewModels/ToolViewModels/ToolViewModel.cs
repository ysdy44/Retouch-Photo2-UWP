using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public abstract class ToolViewModel
    {
        public ToolType Type;

        public abstract void Start(Vector2 point, DrawViewModel viewModel);
        public abstract void Delta(Vector2 point, DrawViewModel viewModel);
        public abstract void Complete(Vector2 point, DrawViewModel viewModel);

        public abstract void Draw(CanvasDrawingSession ds, DrawViewModel viewModel);
    }
}
