using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public sealed partial class PatternGridTool : Page, ITool
    {

        //HorizontalStep
        private void ConstructHorizontalStep1()
        {
            //Button
            this.HorizontalStepTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = PatternGridMode.HorizontalStep;
                else
                    this.TouchBarMode = PatternGridMode.None;
            };

            //Number
            this.HorizontalStepTouchbarSlider.Unit = "";
            this.HorizontalStepTouchbarSlider.NumberMinimum = 5;
            this.HorizontalStepTouchbarSlider.NumberMaximum = 100;
            this.HorizontalStepTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set grid layer step");

                //Selection
                this.SelectionViewModel.PatternGridHorizontalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        var previous = patternGridLayer.HorizontalStep;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            patternGridLayer.IsRefactoringRender = true;
                            patternGridLayer.IsRefactoringIconRender = true;
                            patternGridLayer.HorizontalStep = previous;
                        };

                        //Refactoring
                        patternGridLayer.IsRefactoringRender = true;
                        patternGridLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        patternGridLayer.HorizontalStep = step;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructHorizontalStep2()
        {
            //HorizontalStep
            this.HorizontalStepTouchbarSlider.Value = 30;
            this.HorizontalStepTouchbarSlider.Minimum = 5;
            this.HorizontalStepTouchbarSlider.Maximum = 100;
            this.HorizontalStepTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;
                        patternGridLayer.CacheHorizontalStep();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.HorizontalStepTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //Selection
                this.SelectionViewModel.PatternGridHorizontalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        patternGridLayer.HorizontalStep = step;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.HorizontalStepTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set grid layer step");

                //Selection
                this.SelectionViewModel.PatternGridHorizontalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        var previous = patternGridLayer.StartingHorizontalStep;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            patternGridLayer.IsRefactoringRender = true;
                            patternGridLayer.IsRefactoringIconRender = true;
                            patternGridLayer.HorizontalStep = previous;
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        patternGridLayer.HorizontalStep = step;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}