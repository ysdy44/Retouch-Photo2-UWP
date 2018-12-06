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
    public class ToolViewModel
    {
        public ToolType Type;

        public virtual void Start(Vector2 point, DrawViewModel viewModel)
        {
        }
        public virtual void Delta(Vector2 point, DrawViewModel viewModel)
        {
        }
        public virtual void Complete(Vector2 point, DrawViewModel viewModel)
        {
        }

        public bool IsRender = false;
        public virtual void Render(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }

    }
}
