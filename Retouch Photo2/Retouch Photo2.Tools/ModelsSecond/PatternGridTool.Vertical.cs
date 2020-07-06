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

        //VerticalStep
        private void ConstructVerticalStep1()
        {
            this.VerticalStepTouchbarPicker.Unit = "";
            this.VerticalStepTouchbarPicker.Minimum = 5;
            this.VerticalStepTouchbarPicker.Maximum = 100;
            this.VerticalStepTouchbarPicker.ValueChange += (sender, value) =>
            {
                float verticalStep = (float)value;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    setSelectionViewModel: () => this.SelectionViewModel.PatternGridVerticalStep = verticalStep,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    historyTitle: "Set grid layer step",
                    getHistory: (tLayer) => tLayer.VerticalStep,
                    setHistory: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

        private void ConstructVerticalStep2()
        {
            this.VerticalStepTouchbarSlider.Minimum = 5.0d;
            this.VerticalStepTouchbarSlider.Maximum = 100.0d;
            this.VerticalStepTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>
            (
                layerType: LayerType.PatternGrid,
                cache: (tLayer) => tLayer.CacheVerticalStep()
            );
            this.VerticalStepTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>
            (
                layerType: LayerType.PatternGrid,
                set: (tLayer) => tLayer.VerticalStep = (float)value
            );
            this.VerticalStepTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float verticalStep = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    setSelectionViewModel: () => this.SelectionViewModel.PatternGridVerticalStep = verticalStep,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    historyTitle: "Set grid layer step",
                    getHistory: (tLayer) => tLayer.StartingVerticalStep,
                    setHistory: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

    }
}