using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using System.Numerics;

namespace Retouch_Photo.Tools
{
    public abstract class ICursorTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        

        public override void Start(Vector2 point)
        {
            // Transformer
            if (this.ViewModel.TransformerStart(point)) return;

            this.OperatorStart(point);
        }
        public override void Delta(Vector2 point)
        {
            // Transformer
            if (this.ViewModel.TransformerDelta(point)) return;

            this.OperatorDelta(point);
        }
        public override void Complete(Vector2 point)
        {
            // Transformer
            if (this.ViewModel.TransformerComplete(point)) return;

            this.OperatorComplete(point);
        }

        public override void Draw(CanvasDrawingSession ds)
        { 
            //Transformer      
            if (this.ViewModel.TransformerDraw(ds)) return;

            this.OperatorDraw(ds);           
        }
        
    
        //Operator
        public abstract bool OperatorStart(Vector2 point);
        public abstract bool OperatorDelta(Vector2 point);
        public abstract bool OperatorComplete(Vector2 point);

        public abstract bool OperatorDraw(CanvasDrawingSession ds);
    }
}
