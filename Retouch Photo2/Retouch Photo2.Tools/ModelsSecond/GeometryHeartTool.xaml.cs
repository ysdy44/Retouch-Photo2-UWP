using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
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
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public sealed partial class GeometryHeartTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        

        //@Converter
        private int SpreadNumberConverter(float spread) => (int)(spread * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryHeartTool. 
        /// </summary>
        public GeometryHeartTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSpread1();
            this.ConstructSpread2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }        

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public partial class GeometryHeartTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryHeart");

            this.SpreadTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryHeart_Spread");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryHeart;
        public FrameworkElement Icon { get; } = new GeometryHeartIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryHeartIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryHeartLayer(customDevice)
            {
                Spread = this.SelectionViewModel.GeometryHeartSpread,
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
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public partial class GeometryHeartTool : Page, ITool
    {

        //Spead
        private void ConstructSpread1()
        {
            this.SpreadTouchbarPicker.Unit = "%";
            this.SpreadTouchbarPicker.Minimum = 0;
            this.SpreadTouchbarPicker.Maximum = 100;
            this.SpreadTouchbarPicker.ValueChange += (sender, value) =>
            {
                float spread = (float)value / 100.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryHeartSpread = spread,
                    set: (tLayer) => tLayer.Spread = spread,

                    historyTitle: "Set heart layer spread",
                    getHistory: (tLayer) => tLayer.Spread,
                    setHistory: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

        private void ConstructSpread2()
        {
            this.SpreadTouchbarSlider.Minimum = 0.0d;
            this.SpreadTouchbarSlider.Maximum = 1.0d;
            this.SpreadTouchbarSlider.ValueChangeStarted += (sender, value) =>    this.MethodViewModel.TLayerChangeStarted<GeometryHeartLayer>
            (
                layerType: LayerType.GeometryHeart,
                cache: (tLayer) => tLayer.CacheSpread()
            );
            this.SpreadTouchbarSlider.ValueChangeDelta += (sender, value) =>this.MethodViewModel.TLayerChangeDelta<GeometryHeartLayer>
            (
                layerType: LayerType.GeometryHeart,
                set: (tLayer) => tLayer.Spread = (float)value
            );
            this.SpreadTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float spread = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryHeartSpread = spread,
                    set: (tLayer) => tLayer.Spread = spread,

                    historyTitle: "Set heart layer spread",
                    getHistory: (tLayer) => tLayer.StartingSpread,
                    setHistory: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

    }
}