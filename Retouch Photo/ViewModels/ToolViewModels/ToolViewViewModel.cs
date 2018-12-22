
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
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        public override void Start(Vector2 point)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = this.ViewModel.MatrixTransformer.Position;

            this.ViewModel.Invalidate(isThumbnail: true);
        }
        public override void Delta(Vector2 point)
        {
            this.ViewModel.MatrixTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            this.ViewModel.Invalidate();
        }
        public override void Complete(Vector2 point)
        {
            this.ViewModel.Invalidate(isThumbnail: false);
        }
        
        public override void Draw(CanvasDrawingSession ds)
        {
        }
    }
}


