// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PatternDiagonalTool.
    /// </summary>
    public partial class PatternDiagonalTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.PatternDiagonal;
        public FrameworkElement Icon { get; } = new PatternDiagonalIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new PatternDiagonalIcon()
        };
        public FrameworkElement Page { get; } = new PatternDiagonalPage();


        //@Construct
        /// <summary>
        /// Initializes a PatternDiagonalTool. 
        /// </summary>
        public PatternDiagonalTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new PatternDiagonalLayer(customDevice)
            {
                HorizontalStep = this.SelectionViewModel.PatternDiagonalHorizontalStep,
                Offset = this.SelectionViewModel.PatternDiagonalOffset,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("Tools_PatternDiagonal");
        }

    }


    /// <summary>
    /// Page of <see cref="PatternDiagonalTool"/>.
    /// </summary>
    internal partial class PatternDiagonalPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int OffsetToNumberConverter(float value) => (int)value;
        private int StepToNumberConverter(float value) => (int)value;


        //@Construct
        /// <summary>
        /// Initializes a PatternDiagonalPage. 
        /// </summary>
        public PatternDiagonalPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructOffset1();
            this.ConstructOffset2();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.OffsetButton.CenterContent = resource.GetString("Tools_PatternDiagonal_Offset");
            this.HorizontalStepButton.CenterContent = resource.GetString("Tools_PatternDiagonal_HorizontalStep");
        }
    }

    /// <summary>
    /// Page of <see cref="PatternDiagonalTool"/>.
    /// </summary>
    internal partial class PatternDiagonalPage : Page
    {

        //Offset
        private void ConstructOffset1()
        {
            this.OffsetPicker.Unit = null;
            this.OffsetPicker.Minimum = -100;
            this.OffsetPicker.Maximum = 100;
            this.OffsetPicker.ValueChanged += (sender, value) =>
            {
                float offset = value;
                this.SelectionViewModel.PatternDiagonalOffset = offset;

                this.MethodViewModel.TLayerChanged<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.Offset = offset,

                    historyTitle: "Set diagonal layer offset",
                    getHistory: (tLayer) => tLayer.Offset,
                    setHistory: (tLayer, previous) => tLayer.Offset = previous
                );
            };
        }

        private void ConstructOffset2()
        {
            this.OffsetSlider.Minimum = -100.0d;
            this.OffsetSlider.Maximum = 100.0d;
            this.OffsetSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, cache: (tLayer) => tLayer.CacheOffset());
            this.OffsetSlider.ValueChangeDelta += (sender, value) =>
            {
                float offset = (float)value;
                this.SelectionViewModel.PatternDiagonalOffset = offset;

                this.MethodViewModel.TLayerChangeDelta<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, set: (tLayer) => tLayer.Offset = offset);
            };
            this.OffsetSlider.ValueChangeCompleted += (sender, value) =>
            {
                float offset = (float)value;
                this.SelectionViewModel.PatternDiagonalOffset = offset;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.Offset = offset,

                    historyTitle: "Set diagonal layer offset",
                    getHistory: (tLayer) => tLayer.StartingOffset,
                    setHistory: (tLayer, previous) => tLayer.Offset = previous
                );
            };
        }


        //HorizontalStep
        private void ConstructHorizontalStep1()
        {
            this.HorizontalStepPicker.Unit = null;
            this.HorizontalStepPicker.Minimum = 5;
            this.HorizontalStepPicker.Maximum = 100;
            this.HorizontalStepPicker.ValueChanged += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonalHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChanged<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set diagonal layer horizontal step",
                    getHistory: (tLayer) => tLayer.HorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

        private void ConstructHorizontalStep2()
        {
            this.HorizontalStepSlider.Minimum = 5.0d;
            this.HorizontalStepSlider.Maximum = 100.0d;
            this.HorizontalStepSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, cache: (tLayer) => tLayer.CacheHorizontalStep());
            this.HorizontalStepSlider.ValueChangeDelta += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonalHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeDelta<PatternDiagonalLayer>(layerType: LayerType.PatternDiagonal, set: (tLayer) => tLayer.HorizontalStep = horizontalStep);
            };
            this.HorizontalStepSlider.ValueChangeCompleted += (sender, value) =>
            {
                float horizontalStep = (float)value;
                this.SelectionViewModel.PatternDiagonalHorizontalStep = horizontalStep;

                this.MethodViewModel.TLayerChangeCompleted<float, PatternDiagonalLayer>
                (
                    layerType: LayerType.PatternDiagonal,
                    set: (tLayer) => tLayer.HorizontalStep = horizontalStep,

                    historyTitle: "Set diagonal layer horizontal step",
                    getHistory: (tLayer) => tLayer.StartingHorizontalStep,
                    setHistory: (tLayer, previous) => tLayer.HorizontalStep = previous
                );
            };
        }

    }
}