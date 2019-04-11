using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System.Numerics;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models.CursorTools
{
    public class ITranslation : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        bool IsTranslation;

        public override bool Start(Vector2 point)
        {
            //Translation 
            Layer layer = this.ViewModel.RenderLayer.GetClickedLayer(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            if (layer == null)
            {
                return false;
            }
            else
            {
                this.IsTranslation = true;
                this.ViewModel.CurrentLayer = layer;
                this.ViewModel.TransformerDictionary[TransformerMode.Translation].Start(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);

                return true;
            }
        }
        public override bool Delta(Vector2 point)
        {
            if (this.IsTranslation == false) return false;

            //Translation 
            Layer layer = this.ViewModel.CurrentLayer;
            if (layer == null)
            {
                return false;
            }
            else
            {
                this.ViewModel.TransformerDictionary[TransformerMode.Translation].Delta(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.ViewModel.Invalidate();
                return true;
            }

        }
        public override bool Complete(Vector2 point)
        {
            if (this.IsTranslation == false)return false;
            this.IsTranslation = false;

            //Translation 
            Layer layer = this.ViewModel.CurrentLayer;
            if (layer == null)
            {
                return false;
            }
            else
            {
                this.ViewModel.TransformerDictionary[TransformerMode.Translation].Complete(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);

                return true;
            }
        }

        public override bool Draw(CanvasDrawingSession ds)
        {
            return false;
        }
    }
}