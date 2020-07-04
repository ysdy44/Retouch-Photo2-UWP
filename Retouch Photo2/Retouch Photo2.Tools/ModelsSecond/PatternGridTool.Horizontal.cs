using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
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
                float horizontalStep = (float)value;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    setSelectionViewModel: () => this.SelectionViewModel.PatternGridHorizontalStep = horizontalStep,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set grid layer step",
                    getHistory: (tLayer) => tLayer.HorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
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
                this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>
                (
                    LayerType.PatternGrid,
                    (tLayer) => tLayer.CacheHorizontalStep()
                );
            };
            this.HorizontalStepTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                if (horizontalStep < 5.0f) horizontalStep = 5.0f;
                if (horizontalStep > 100.0f) horizontalStep = 100.0f;

                this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>
                (
                    LayerType.PatternGrid,
                    (tLayer) => tLayer.HorizontalStep = horizontalStep
                );
            };
            this.HorizontalStepTouchbarSlider.ValueChangeCompleted += (sender, value2) =>
            {
                float horizontalStep = (float)value2;
                if (horizontalStep < 5.0f) horizontalStep = 5.0f;
                if (horizontalStep > 100.0f) horizontalStep = 100.0f;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    setSelectionViewModel: () => this.SelectionViewModel.PatternGridHorizontalStep = horizontalStep,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set grid layer step",
                    getHistory: (tLayer) => tLayer.StartingHorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

    }
}