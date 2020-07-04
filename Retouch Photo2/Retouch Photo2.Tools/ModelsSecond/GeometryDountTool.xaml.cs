using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
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
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        private bool TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case false:
                        this.HoleRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.HoleRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.HoleRadiusTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int HoleRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double HoleRadiusValueConverter(float innerRadius) => innerRadius * 100d;


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
            this.TouchBarMode = false;
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

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryDount");
            this._button.Style = this.IconSelectedButtonStyle;

            this.HoleRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDount_HoleRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDount;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryDountIcon();
        readonly Button _button = new Button { Tag = new GeometryDountIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryDountLayer(customDevice)
            {
                HoleRadius = this.SelectionViewModel.GeometryDountHoleRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {

        //HoleRadius
        private void ConstructHoleRadius1()
        {
            //Button
            this.HoleRadiusTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.HoleRadiusTouchbarSlider.Unit = "%";
            this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
            this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
            this.HoleRadiusTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryDountHoleRadius = holeRadius,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    historyTitle: "Set dount layer hole radius",
                    getHistory: (tLayer) => tLayer.HoleRadius,
                    setHistory: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }
        private void ConstructHoleRadius2()
        {
            //Value
            this.HoleRadiusTouchbarSlider.Unit = "%";
            this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
            this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
            this.HoleRadiusTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                this.MethodViewModel.TLayerChangeStarted<GeometryDountLayer>
                (
                    LayerType.GeometryDount,
                    (tLayer) => tLayer.CacheHoleRadius()
                );
            };
            this.HoleRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                this.MethodViewModel.TLayerChangeDelta<GeometryDountLayer>
                (
                    LayerType.GeometryDount,
                    (tLayer) => tLayer.HoleRadius = holeRadius
                );
            };
            this.HoleRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryDountHoleRadius = holeRadius,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    historyTitle: "Set dount layer hole radius",
                    getHistory: (tLayer) => tLayer.StartingHoleRadius,
                    setHistory: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }

    }
}