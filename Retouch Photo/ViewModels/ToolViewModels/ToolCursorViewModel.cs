using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels;
using Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolCursorViewModel : ToolViewModel
    {

        Layer CurrentLayer;

        CursorMode Mode = CursorMode.None;

        #region ViewModel2


        ToolViewModel2 ViewModel2
        {
            get
            {
                switch (this.Mode)
                {
                    case CursorMode.Translation: return this.TranslationViewModel;
                    case CursorMode.Rotation: return this.RotationViewModel;

                    case CursorMode.SkewLeft: return this.SkewLeftViewModel;
                    case CursorMode.SkewTop: return this.SkewTopViewModel;
                    case CursorMode.SkewRight: return this.SkewRightViewModel;
                    case CursorMode.SkewBottom: return this.SkewBottomViewModel;

                    default: return this.NoneViewModel;
                }
            }
        }

        readonly ToolCursorNoneViewModel NoneViewModel = new ToolCursorNoneViewModel();
        readonly ToolCursorTranslationViewModel TranslationViewModel = new ToolCursorTranslationViewModel();
        readonly ToolCursorRotationViewModel RotationViewModel = new ToolCursorRotationViewModel();

        readonly ToolCursorSkewLeftViewModel SkewLeftViewModel = new ToolCursorSkewLeftViewModel();
        readonly ToolCursorSkewTopViewModel SkewTopViewModel = new ToolCursorSkewTopViewModel();
        readonly ToolCursorSkewRightViewModel SkewRightViewModel = new ToolCursorSkewRightViewModel();
        readonly ToolCursorSkewBottomViewModel SkewBottomViewModel = new ToolCursorSkewBottomViewModel();


        #endregion


        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.CurrentLayer = viewModel.RenderLayer.CurrentLayer;


            //CursorMode
            if (this.CurrentLayer != null)
            {
                this.Mode = Transformer.ContainsNodeMode(point, this.CurrentLayer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix, viewModel.KeyCtrl);

                if (this.Mode!= CursorMode.None)
                {
                    this.ViewModel2.Start(point, this.CurrentLayer, viewModel);
                    return;
                }
            }


            //Translation
            foreach (Layer layer in viewModel.RenderLayer.Layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                Vector2 canvasPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
                if (Transformer.ContainsBound(canvasPoint, layer.Transformer))
                {
                    this.CurrentLayer = viewModel.RenderLayer.CurrentLayer = layer;
                    this.Mode = CursorMode.Translation;

                    this.ViewModel2.Start(point, this.CurrentLayer, viewModel);
                    return;
                }
            }
        }
        
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            this.ViewModel2.Delta(point, this.CurrentLayer, viewModel);

            viewModel.Invalidate();
        }
        
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            this.ViewModel2.Complete(point, this.CurrentLayer, viewModel);

            this.Mode = CursorMode.None;
            viewModel.Invalidate();
        }
        

        
        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            this.ViewModel2.Draw(ds, this.CurrentLayer, viewModel);
        }

    }
}


