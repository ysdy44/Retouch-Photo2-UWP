using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PatternDiagonalTool.
    /// </summary>
    public sealed partial class PatternDiagonalTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int Converter(float value) => (int)value;


        //@Construct
        /// <summary>
        /// Initializes a PatternDiagonalTool. 
        /// </summary>
        public PatternDiagonalTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructOffset1();
            this.ConstructOffset2();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternDiagonalTool.
    /// </summary>
    public partial class PatternDiagonalTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/PatternDiagonal");

            this.OffsetButton.CenterContent = resource.GetString("/ToolsSecond/PatternDiagonal_Offset");
            this.HorizontalStepButton.CenterContent = resource.GetString("/ToolsSecond/PatternDiagonal_HorizontalStep");
        }


        //@Content
        public ToolType Type => ToolType.PatternDiagonal;
        public FrameworkElement Icon { get; } = new PatternDiagonalIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new PatternDiagonalIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new PatternDiagonalLayer(customDevice)
            {
                HorizontalStep = this.SelectionViewModel.PatternDiagonalHorizontalStep,
                Offset = this.SelectionViewModel.PatternDiagonalOffset,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternDiagonalTool.
    /// </summary>
    public partial class PatternDiagonalTool : Page, ITool
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