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
    internal enum PatternGridMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Horizontal Step. </summary>
        HorizontalStep,

        /// <summary> Vertical Step. </summary>
        VerticalStep
    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.PatternGrid;
        public FrameworkElement Icon { get; } = new PatternGridIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new PatternGridIcon()
        };
        public FrameworkElement Page { get; } = new PatternGridPage();


        //@Construct
        /// <summary>
        /// Initializes a PatternGridTool. 
        /// </summary>
        public PatternGridTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new PatternGridLayer(customDevice)
            {
                HorizontalStep = this.SelectionViewModel.PatternGridHorizontalStep,
                VerticalStep = this.SelectionViewModel.PatternGridVerticalStep,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/PatternGrid");
        }

    }


    /// <summary>
    /// Page of <see cref="PatternGridTool"/>.
    /// </summary>
    internal partial class PatternGridPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int StepToNumberConverter(float value) => (int)value;
        private Visibility HorizontalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Vertical ? Visibility.Collapsed : Visibility.Visible;
        private Visibility VerticalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Horizontal ? Visibility.Collapsed : Visibility.Visible;


        //@Construct
        /// <summary>
        /// Initializes a PatternGridPage. 
        /// </summary>
        public PatternGridPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructGridType();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();

            this.ConstructVerticalStep1();
            this.ConstructVerticalStep2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TypeTextBlock.Text = resource.GetString("/ToolsSecond/PatternGrid_Type");
            this.HorizontalStepButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Horizontal");
            this.VerticalStepButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Vertical");
        }
    }

    /// <summary>
    /// Page of <see cref="PatternGridTool"/>.
    /// </summary>
    internal partial class PatternGridPage : Page
    {

        //GridType
        private void ConstructGridType()
        {
            this.PatternGridTypeComboBox.TypeChanged += (s, type) =>
            {
                PatternGridType gridType = (PatternGridType)type;
                this.SelectionViewModel.PatternGridType = gridType;

                this.MethodViewModel.TLayerChanged<PatternGridType, PatternGridLayer>
                (
                    layerType: LayerType.PatternGrid,
                    set: (tLayer) => tLayer.GridType = gridType,

                    historyTitle: "Set grid layer type",
                    getHistory: (tLayer) => tLayer.GridType,
                    setHistory: (tLayer, previous) => tLayer.GridType = previous
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