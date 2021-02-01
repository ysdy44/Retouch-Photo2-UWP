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
    /// <see cref="GeometryTool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryDount;
        public FrameworkElement Icon { get; } = new GeometryDountIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryDountIcon()
        };
        public FrameworkElement Page { get; } = new GeometryDountPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryDountTool. 
        /// </summary>
        public GeometryDountTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryDountLayer(customDevice)
            {
                HoleRadius = this.SelectionViewModel.GeometryDountHoleRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryDount");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryDountTool"/>.
    /// </summary>
    internal partial class GeometryDountPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int HoleRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryDountPage. 
        /// </summary>
        public GeometryDountPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructHoleRadius1();
            this.ConstructHoleRadius2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.HoleRadiusButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDount_HoleRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryDountTool"/>.
    /// </summary>
    internal partial class GeometryDountPage : Page
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