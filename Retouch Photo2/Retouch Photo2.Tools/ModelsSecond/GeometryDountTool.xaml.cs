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
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public sealed partial class GeometryDountTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        
        //@Converter
        private int HoleRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryDountTool. 
        /// </summary>
        public GeometryDountTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructHoleRadius1();
            this.ConstructHoleRadius2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryDount");

            this.HoleRadiusButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDount_HoleRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDount;
        public FrameworkElement Icon { get; } = new GeometryDountIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryDountIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryDountLayer(customDevice)
            {
                HoleRadius = this.SelectionViewModel.GeometryDountHoleRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {

        //HoleRadius
        private void ConstructHoleRadius1()
        {
            this.HoleRadiusPicker.Unit = "%";
            this.HoleRadiusPicker.Minimum = 0;
            this.HoleRadiusPicker.Maximum = 100;
            this.HoleRadiusPicker.ValueChanged += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    historyTitle: "Set dount layer hole radius",
                    getHistory: (tLayer) => tLayer.HoleRadius,
                    setHistory: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }

        private void ConstructHoleRadius2()
        {
            this.HoleRadiusSlider.Minimum = 0.0d;
            this.HoleRadiusSlider.Maximum = 1.0d;
            this.HoleRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryDountLayer>(layerType: LayerType.GeometryDount, cache: (tLayer) => tLayer.CacheHoleRadius());
            this.HoleRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float holeRadius = (float)value;
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryDountLayer>(layerType: LayerType.GeometryDount, set: (tLayer) => tLayer.HoleRadius = holeRadius);
            };
            this.HoleRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float holeRadius = (float)value;
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    historyTitle: "Set dount layer hole radius",
                    getHistory: (tLayer) => tLayer.StartingHoleRadius,
                    setHistory: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }

    }
}