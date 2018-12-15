
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
            this.rightStartPosition = viewModel.MatrixTransformer.Position;

            viewModel.Invalidate(isThumbnail: true);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.MatrixTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            viewModel.Invalidate();
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Invalidate(isThumbnail: false);
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }

    }
}


