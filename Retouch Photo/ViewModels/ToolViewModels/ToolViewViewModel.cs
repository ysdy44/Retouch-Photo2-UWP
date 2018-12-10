
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolViewViewModel : ToolViewModel
    {

        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = viewModel.Transformer.Position;

            viewModel.Invalidate(isDottedLineRender: true);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Transformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            viewModel.Invalidate(isDottedLineRender: true);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Invalidate(isDottedLineRender: true);
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }

    }
}


