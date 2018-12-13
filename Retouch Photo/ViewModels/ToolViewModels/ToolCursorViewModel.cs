using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
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
    public class ToolCursorViewModel : ToolViewModel
    {

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Transformer.Position = point;

            viewModel.Invalidate(isDottedLineRender: true, isThumbnail: true);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Transformer.Position = point;

            viewModel.Invalidate(isDottedLineRender: true);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Invalidate(isDottedLineRender: true, isThumbnail: false);
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }


  



    }
}


