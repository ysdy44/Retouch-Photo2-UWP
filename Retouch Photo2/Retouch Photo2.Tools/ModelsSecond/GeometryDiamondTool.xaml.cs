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
    /// <see cref="GeometryTool"/>'s GeometryDiamondTool.
    /// </summary>
    public partial class GeometryDiamondTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryDiamond;
        public FrameworkElement Icon { get; } = new GeometryDiamondIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryDiamondIcon()
        };
        public FrameworkElement Page { get; } = new GeometryDiamondPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryDiamondTool. 
        /// </summary>
        public GeometryDiamondTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryDiamondLayer(customDevice)
            {
                Mid = this.SelectionViewModel.GeometryDiamondMid,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryDiamond");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryDiamondTool"/>.
    /// </summary>
    public partial class GeometryDiamondPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int MidToNumberConverter(float mid) => (int)(mid * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryDiamondPage. 
        /// </summary>
        public GeometryDiamondPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructMid1();
            this.ConstructMid2();
            this.ConstructMirror();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryDiamond_Mirror");
            this.MidButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDiamond_Mid");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryDiamondTool"/>.
    /// </summary>
    public partial class GeometryDiamondPage : Page
    {

        //Mid
        private void ConstructMid1()
        {
            this.MidPicker.Unit = "%";
            this.MidPicker.Minimum = 0;
            this.MidPicker.Maximum = 100;
            this.MidPicker.ValueChanged += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                this.SelectionViewModel.GeometryDiamondMid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.Mid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMid2()
        {
            this.MidSlider.Minimum = 0.0d;
            this.MidSlider.Maximum = 1.0d;
            this.MidSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, cache: (tLayer) => tLayer.CacheMid());
            this.MidSlider.ValueChangeDelta += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamondMid = mid;

                this.MethodViewModel.TLayerChangeDelta<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, set: (tLayer) => tLayer.Mid = mid);
            };
            this.MidSlider.ValueChangeCompleted += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamondMid = mid;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.StartingMid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float mid = 1.0f - this.SelectionViewModel.GeometryDiamondMid;
                this.SelectionViewModel.GeometryDiamondMid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = 1.0f - tLayer.Mid,

                    historyTitle: "Set diamond layer mid",
                    getHistory: (tLayer) => tLayer.Mid,
                    setHistory: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

    }
}