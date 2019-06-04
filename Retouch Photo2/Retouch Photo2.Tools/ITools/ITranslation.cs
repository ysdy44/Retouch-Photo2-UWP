using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.ITools
{
    public class ITranslation : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        bool IsTranslation;

        public override bool Start(Vector2 point)
        {
            //Translation 
            Layer layer = this.ViewModel.RenderLayer.GetClickedLayer(point, this.ViewModel.MatrixTransformer.Matrix);
            if (layer == null)
            {
                return false;
            }
            else
            {
                this.IsTranslation = true;

                this.ViewModel.RenderLayer.Selected(layer);
                this.ViewModel.SetLayer(layer);

                this.ViewModel.TransformerDictionary[TransformerMode.Translation].Start(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);

                return true;
            }
        }
        public override bool Delta(Vector2 point)
        {
            if (this.IsTranslation == false) return false;

            //Translation 
            Layer layer = this.ViewModel.Layer;
            if (layer == null)
            {
                return false;
            }
            else
            {
                this.ViewModel.TransformerDictionary[TransformerMode.Translation].Delta(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);

                this.ViewModel.Transformer = layer.Transformer;//Transformer
                this.ViewModel.Invalidate();
                return true;
            }

        }
        public override bool Complete(Vector2 point)
        {
            if (this.IsTranslation == false)return false;
            this.IsTranslation = false;

            //Translation 
            Layer layer = this.ViewModel.Layer;
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