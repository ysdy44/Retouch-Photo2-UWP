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
            this.VerticalStepPicker.Unit = null;
            this.VerticalStepPicker.Minimum = 5;
            this.VerticalStepPicker.Maximum = 100;
            this.VerticalStepPicker.ValueChanged += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGridVerticalStep = verticalStep;

                this.MethodViewModel.TLayerChanged<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    historyTitle: "Set grid layer vertical step",
                    getHistory: (tLayer) => tLayer.VerticalStep,
                    setHistory: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

        private void ConstructVerticalStep2()
        {
            this.VerticalStepSlider.Minimum = 5.0d;
            this.VerticalStepSlider.Maximum = 100.0d;
            this.VerticalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternGridLayer>(layerType: LayerType.PatternGrid, cache: (tLayer) => tLayer.CacheVerticalStep());
            this.VerticalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGridVerticalStep = verticalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternGridLayer>(layerType: LayerType.PatternGrid, set: (tLayer) => tLayer.VerticalStep = verticalStep);
            };
            this.VerticalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float verticalStep = (float)value;
                this.SelectionViewModel.PatternGridVerticalStep = verticalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.VerticalStep = verticalStep,

                    historyTitle: "Set grid layer vertical step",
                    getHistory: (tLayer) => tLayer.StartingVerticalStep,
                    setHistory: (tLayer, previous) => tLayer.VerticalStep = previous
                );
            };
        }

    }
}