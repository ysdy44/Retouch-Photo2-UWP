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
    /// <see cref="GeometryTool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryTriangle;
        public FrameworkElement Icon { get; } = new GeometryTriangleIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryTriangleIcon()
        };
        public FrameworkElement Page { get; } = new GeometryTrianglePage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryTriangleTool. 
        /// </summary>
        public GeometryTriangleTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryTriangleLayer(customDevice)
            {
                Center = this.SelectionViewModel.GeometryTriangleCenter,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryTriangle");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryTriangleTool"/>.
    /// </summary>
    internal partial class GeometryTrianglePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int CenterToNumberConverter(float center) => (int)(center * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryTrianglePage. 
        /// </summary>
        public GeometryTrianglePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCenter1();
            this.ConstructCenter2();
            this.ConstructMirror();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CenterButton.CenterContent = resource.GetString("/ToolsSecond/GeometryTriangle_Center");
            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryTriangle_Mirror");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryTriangleTool"/>.
    /// </summary>
    internal partial class GeometryTrianglePage : Page
    {

        //Center
        private void ConstructCenter1()
        {
            this.CenterPicker.Unit = "%";
            this.CenterPicker.Minimum = 0;
            this.CenterPicker.Maximum = 100;
            this.CenterPicker.ValueChanged += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                this.SelectionViewModel.GeometryTriangleCenter = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    layerType: LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    historyTitle: "Set triangle layer center",
                    getHistory: (tLayer) => tLayer.Center,
                    setHistory: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructCenter2()
        {
            this.CenterSlider.Minimum = 0.0d;
            this.CenterSlider.Maximum = 1.0d;
            this.CenterSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TLayerChangeStarted<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, cache: (tLayer) => tLayer.CacheCenter());
            this.CenterSlider.ValueChangeDelta += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangleCenter = center;

                this.MethodViewModel.TLayerChangeDelta<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, set: (tLayer) => tLayer.Center = center);
            };
            this.CenterSlider.ValueChangeCompleted += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangleCenter = center;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    historyTitle: "Set triangle layer center",
                    getHistory: (tLayer) => tLayer.StartingCenter,
                    setHistory: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float center = 1.0f - this.SelectionViewModel.GeometryTriangleCenter;
                this.SelectionViewModel.GeometryTriangleCenter = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = 1.0f - tLayer.Center,

                    historyTitle: "Set triangle layer center",
                    getHistory: (tLayer) => tLayer.Center,
                    setHistory: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

    }
}