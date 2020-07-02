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

        //VerticalStep
        private void ConstructVerticalStep1()
        {
            //Button
            this.VerticalStepTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = PatternGridMode.VerticalStep;
                else
                    this.TouchBarMode = PatternGridMode.None;
            };

            //Number
            this.VerticalStepTouchbarSlider.Unit = "";
            this.VerticalStepTouchbarSlider.NumberMinimum = 5;
            this.VerticalStepTouchbarSlider.NumberMaximum = 100;
            this.VerticalStepTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set grid layer step");

                //Selection
                this.SelectionViewModel.PatternGridVerticalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        var previous = patternGridLayer.VerticalStep;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            patternGridLayer.IsRefactoringRender = true;
                            patternGridLayer.IsRefactoringIconRender = true;
                            patternGridLayer.VerticalStep = previous;
                        };

                        //Refactoring
                        patternGridLayer.IsRefactoringRender = true;
                        patternGridLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        patternGridLayer.VerticalStep = step;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructVerticalStep2()
        {
            //VerticalStep
            this.VerticalStepTouchbarSlider.Value = 30;
            this.VerticalStepTouchbarSlider.Minimum = 5;
            this.VerticalStepTouchbarSlider.Maximum = 100;
            this.VerticalStepTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;
                        patternGridLayer.CacheVerticalStep();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.VerticalStepTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //Selection
                this.SelectionViewModel.PatternGridVerticalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        patternGridLayer.VerticalStep = step;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.VerticalStepTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float step = (float)value;
                if (step < 5.0f) step = 5.0f;
                if (step > 100.0f) step = 100.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set grid layer step");

                //Selection
                this.SelectionViewModel.PatternGridVerticalStep = step;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.PatternGrid)
                    {
                        PatternGridLayer patternGridLayer = (PatternGridLayer)layer;

                        var previous = patternGridLayer.StartingVerticalStep;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            patternGridLayer.IsRefactoringRender = true;
                            patternGridLayer.IsRefactoringIconRender = true;
                            patternGridLayer.VerticalStep = previous;
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        patternGridLayer.VerticalStep = step;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}