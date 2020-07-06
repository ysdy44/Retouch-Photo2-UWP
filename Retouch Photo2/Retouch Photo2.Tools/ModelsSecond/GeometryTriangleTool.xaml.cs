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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int CenterNumberConverter(float center) => (int)(center * 100.0f);
        

        //@Construct
        /// <summary>
        /// Initializes a GeometryTriangleTool. 
        /// </summary>
        public GeometryTriangleTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCenter1();
            this.ConstructCenter2();
            this.ConstructMirror();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public sealed partial class GeometryTriangleTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryTriangle");

            this.CenterTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryTriangle_Center");
            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryTriangle_Mirror");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryTriangle;
        public FrameworkElement Icon { get; } = new GeometryTriangleIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryTriangleIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryTriangleLayer(customDevice)
            {
                Center = this.SelectionViewModel.GeometryTriangleCenter,
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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public sealed partial class GeometryTriangleTool : Page, ITool
    {

        //Center
        private void ConstructCenter1()
        {
            this.CenterTouchbarPicker.Unit = "%";
            this.CenterTouchbarPicker.Minimum = 0;
            this.CenterTouchbarPicker.Maximum = 100;
            this.CenterTouchbarPicker.ValueChange += (sender, value) =>
            {
                float center = (float)value / 100.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    layerType: LayerType.GeometryTriangle,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryTriangleCenter = center,
                    set: (tLayer) => tLayer.Center = center,

                    historyTitle: "Set triangle layer center",
                    getHistory: (tLayer) => tLayer.Center = center,
                    setHistory: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructCenter2()
        {
            this.CenterTouchbarSlider.Minimum = 0.0d;
            this.CenterTouchbarSlider.Maximum = 1.0d;
            this.CenterTouchbarSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TLayerChangeStarted<GeometryTriangleLayer>
            (
                layerType: LayerType.GeometryTriangle,
                cache: (tLayer) => tLayer.CacheCenter()
            );
            this.CenterTouchbarSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.TLayerChangeDelta<GeometryTriangleLayer>
            (
                layerType: LayerType.GeometryTriangle,
                set: (tLayer) => tLayer.Center = (float)value
            );
            this.CenterTouchbarSlider.ValueChangeCompleted += (s, value) =>
            {
                float center = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryTriangleCenter = center,
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

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryTriangleCenter = center,
                    set: (tLayer) => tLayer.Center = 1.0f - tLayer.Center,

                    historyTitle: "Set triangle layer center",
                    getHistory: (tLayer) => tLayer.Center,
                    setHistory: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

    }
}