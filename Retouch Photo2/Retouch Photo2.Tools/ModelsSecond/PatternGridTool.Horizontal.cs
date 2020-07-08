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
            this.HorizontalStepPicker.Unit = null;
            this.HorizontalStepPicker.Minimum = 5;
            this.HorizontalStepPicker.Maximum = 100;
            this.HorizontalStepPicker.ValueChange += (sender, value) =>
            {
                float horizontalStep = value;
                this.SelectionViewModel.PatternGridHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set grid layer horizontal step",
                    getHistory: (tLayer) => tLayer.HorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

        private void ConstructHorizontalStep2()
        {
            this.HorizontalStepSlider.Minimum = 5.0d;
            this.HorizontalStepSlider.Maximum = 100.0d;
            this.HorizontalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>(layerType: LayerType.PatternGrid, cache: (tLayer) => tLayer.CacheHorizontalStep());
            this.HorizontalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternGridHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>(layerType: LayerType.PatternGrid, set: (tLayer) => tLayer.HorizontalStep = horizontalStep);
            };
            this.HorizontalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternGridHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set grid layer horizontal step",
                    getHistory: (tLayer) => tLayer.StartingHorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

    }
}