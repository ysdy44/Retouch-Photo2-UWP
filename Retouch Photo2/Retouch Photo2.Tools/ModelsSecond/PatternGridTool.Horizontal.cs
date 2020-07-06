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
            this.HorizontalStepTouchbarPicker.Unit = "";
            this.HorizontalStepTouchbarPicker.Minimum = 5;
            this.HorizontalStepTouchbarPicker.Maximum = 100;
            this.HorizontalStepTouchbarPicker.ValueChange += (sender, value) =>
            {
                float horizontalStep = value;

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
            this.HorizontalStepTouchbarSlider.Minimum = 5.0d;
            this.HorizontalStepTouchbarSlider.Maximum = 100.0d;
            this.HorizontalStepTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>
            (
                LayerType.PatternGrid,
                (tLayer) => tLayer.CacheHorizontalStep()
            );
            this.HorizontalStepTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>
            (
                LayerType.PatternGrid,
                (tLayer) => tLayer.HorizontalStep = (float)value
            );
            this.HorizontalStepTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float horizontalStep = (float)value;

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