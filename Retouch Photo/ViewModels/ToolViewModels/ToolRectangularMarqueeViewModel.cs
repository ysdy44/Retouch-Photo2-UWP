using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
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
    public class ToolRectangularMarqueeViewModel : ToolViewModel
    {
        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            //[临时删掉]    viewModel.MarqueeTool.Tool = MarqueeToolType.Rectangular;

            //[临时删掉]    viewModel.MarqueeTool.Operator_Start(point, viewModel.Transformer.InversionMatrix);
            viewModel.Invalidate();
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            //[临时删掉]      viewModel.MarqueeTool.Operator_Delta(point, viewModel.Transformer.InversionMatrix);
            viewModel.Invalidate();
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            //[临时删掉]    viewModel.MarqueeTool.Operator_Complete(point, viewModel.Transformer.InversionMatrix);
            viewModel.Invalidate();
        }



        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            //[临时删掉]     viewModel.MarqueeTool.Draw(viewModel.CanvasControl, ds, viewModel.Transformer.Matrix);
        }


    }
}


