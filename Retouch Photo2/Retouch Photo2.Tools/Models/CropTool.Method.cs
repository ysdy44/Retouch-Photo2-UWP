using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CropTool.
    /// </summary>
    public sealed partial class CropTool : ITool
    {

        private Layerage GetTransformerLayer(Vector2 startingPoint, Matrix3x2 matrix)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;

                case ListViewSelectionMode.Single:
                    {
                        Layerage layerage = this.SelectionViewModel.SelectionLayerage;
                        if (layerage == null) return null;

                        //Transformer
                        Transformer transformer = layerage.GetActualTransformer();
                        this.TransformerMode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix, false);

                        return layerage;
                    }

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        //Transformer
                        Transformer transformer = layerage.GetActualTransformer();
                        TransformerMode mode = Transformer.ContainsNodeMode(startingPoint, transformer, matrix, false);
                        if (mode != TransformerMode.None)
                        {
                            this.TransformerMode = mode;
                            return layerage;
                        }
                    }
                    break;
            }
            return null;
        }

        private void CropStarted(ILayer firstLayer)
        {
            firstLayer.Transform.CropTransformer = firstLayer.Transform.Transformer;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetTransform_IsCrop);

            //History
            var previous = firstLayer.Transform.IsCrop;
            history.UndoAction += () =>
            {
                ILayer firstLayer2 = firstLayer;

                firstLayer2.Transform.IsCrop = previous;
            };

            //History
            this.ViewModel.HistoryPush(history);

            firstLayer.Transform.IsCrop = true;
        }

        private void CropDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            ILayer layer = this.Layerage.Self;
            if (this.IsMove == false)//Transformer
            {
                //Transformer
                Transformer transformer = Transformer.Controller(this.TransformerMode, canvasStartingPoint, canvasPoint, layer.Transform.StartingCropTransformer, this.IsRatio, this.IsCenter, this.IsStepFrequency);

                //Refactoring
                layer.IsRefactoringRender = true;
                this.Layerage.RefactoringParentsRender();
                layer.Transform.CropTransformer = transformer;
            }
            else//Move
            {
                Vector2 canvasMove = canvasPoint - canvasStartingPoint;

                //Refactoring
                layer.IsRefactoringRender = true;
                this.Layerage.RefactoringParentsRender();
                layer.Transform.CropTransformAdd(canvasMove);
            }
        }

        private void CropComplete()
        {
            ILayer layer = this.Layerage.Self;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetTransform_CropTransformer);

            //History
            var previous = layer.Transform.StartingCropTransformer;
            history.UndoAction += () =>
            {
                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layer.Transform.CropTransformer = previous;
            };

            //History
            this.ViewModel.HistoryPush(history);

            //Refactoring
            layer.IsRefactoringRender = true;
            layer.IsRefactoringIconRender = true;
            this.Layerage.RefactoringParentsRender();
            this.Layerage.RefactoringParentsIconRender();
        }

    }
}