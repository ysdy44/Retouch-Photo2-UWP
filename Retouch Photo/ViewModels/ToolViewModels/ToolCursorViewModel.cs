using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels;
using Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolCursorViewModel : ToolViewModel
    {
        Vector2 LayerStartPostion;
        Vector2 StartPoint;

        Layer CurrentLayer;

        CursorMode Mode = CursorMode.None;
        readonly ToolCursorTranslationViewModel TranslationViewModel = new ToolCursorTranslationViewModel();
        readonly ToolCursorRotationViewModel RotationViewModel = new ToolCursorRotationViewModel();
        readonly ToolCursorSkewTopViewModel SkewTopViewModel = new ToolCursorSkewTopViewModel();


        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.CurrentLayer = viewModel.RenderLayer.CurrentLayer;

            if (this.CurrentLayer == null)
            {
                //Translation
                foreach (Layer layer in viewModel.RenderLayer.Layers)
                {
                    if (layer.IsVisual == false || layer.Opacity == 0) continue;

                    Vector2 canvasPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
                    if (Transformer.ContainsBound(canvasPoint, layer.Transformer))
                    {
                        this.TranslationViewModel.Start(point, layer, viewModel);
                        return;
                    }
                }
                return;
            }



            this.Mode = Transformer.ContainsNodeMode
             (
                 point: point,
                 transformer: this.CurrentLayer.Transformer,
                 canvasToVirtualToControlMatrix: viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix,
                 canvasPoint: Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix),
                isCtrl: viewModel.KeyCtrl
             );

            switch (this.Mode)
            {
                case CursorMode.Translation:
                    this.TranslationViewModel.Start(point,  this.CurrentLayer, viewModel);
                    return;
                case CursorMode.Rotation:
                    this.RotationViewModel.Start(point, this.CurrentLayer, viewModel);
                    return;

                case CursorMode.SkewTop:
                    this.SkewTopViewModel.Start(point, this.CurrentLayer, viewModel);
                    return;
                default: break;
            }

            viewModel.Invalidate();
        }


        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            switch (this.Mode)
            {
                case CursorMode.Translation:
                    this.TranslationViewModel.Delta(point, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Delta(point, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.SkewTop:
                    this.SkewTopViewModel.Delta(point, this.CurrentLayer, viewModel);
                    break;
                default:
                    break;
            }

            viewModel.Invalidate();
        }


        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            switch (this.Mode)
            {
                case CursorMode.Translation:
                    this.TranslationViewModel.Complete(point, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Complete(point, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.SkewTop:
                    this.SkewTopViewModel.Complete(point, this.CurrentLayer, viewModel);
                    break;
                default:
                    break;
            }
            this.Mode = CursorMode.None;

            viewModel.Invalidate();
        }



        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            switch (this.Mode)
            {
                case CursorMode.None:
                    if (viewModel.KeyCtrl) Transformer.DrawBoundNodesWithSkew(ds, this.CurrentLayer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                    else Transformer.DrawBoundNodesWithRotation(ds, this.CurrentLayer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                    break;
                case CursorMode.Translation:
                    this.TranslationViewModel.Draw(ds, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Draw(ds, this.CurrentLayer, viewModel);
                    break;
                case CursorMode.SkewTop:
                    this.SkewTopViewModel.Draw(ds, this.CurrentLayer, viewModel);
                    break;
                default:
                    break;
            }
        }


    }
}


