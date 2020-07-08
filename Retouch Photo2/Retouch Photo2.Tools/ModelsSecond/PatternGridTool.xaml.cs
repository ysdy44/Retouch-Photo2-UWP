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
    public sealed partial class PatternGridTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int Converter(float value) => (int)value; 
        private Visibility HorizontalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Vertical ? Visibility.Collapsed : Visibility.Visible;
        private Visibility VerticalStepVisibilityConverter(PatternGridType value) => value == PatternGridType.Horizontal ? Visibility.Collapsed : Visibility.Visible;


        //@Construct
        /// <summary>
        /// Initializes a PatternGridTool. 
        /// </summary>
        public PatternGridTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructGridType();

            this.ConstructHorizontalStep1();
            this.ConstructHorizontalStep2();

            this.ConstructVerticalStep1();
            this.ConstructVerticalStep2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/PatternGrid");

            this.TypeTextBlock.Text = resource.GetString("/ToolsSecond/PatternGrid_Type");
            this.HorizontalStepButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Horizontal");
            this.VerticalStepButton.CenterContent = resource.GetString("/ToolsSecond/PatternGrid_Vertical");
        }


        //@Content
        public ToolType Type => ToolType.PatternGrid;
        public FrameworkElement Icon { get; } = new PatternGridIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new PatternGridIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new PatternGridLayer(customDevice)
            {
                HorizontalStep = this.SelectionViewModel.PatternGridHorizontalStep,
                VerticalStep = this.SelectionViewModel.PatternGridVerticalStep,
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
    /// <see cref="ITool"/>'s PatternGridTool.
    /// </summary>
    public partial class PatternGridTool : Page, ITool
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


    }
}