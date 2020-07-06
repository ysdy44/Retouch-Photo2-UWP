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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryPieTool. 
        /// </summary>
        public GeometryPieTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public sealed partial class GeometryPieTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryPie");

            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPie;
        public FrameworkElement Icon { get; } = new GeometryPieIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryPieIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPieLayer(customDevice)
            {
                SweepAngle = this.SelectionViewModel.GeometryPieSweepAngle,
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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : Page, ITool
    {
        //SweepAngle
        private void ConstructSweepAngle1()
        {
            this.SweepAngleTouchbarPicker.Unit = "º";
            this.SweepAngleTouchbarPicker.Minimum = 0;
            this.SweepAngleTouchbarPicker.Maximum = 360;
            this.SweepAngleTouchbarPicker.ValueChange += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                
                this.MethodViewModel.TLayerChanged<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructSweepAngle2()
        {
            this.SweepAngleTouchbarSlider.Minimum = 0.0d;
            this.SweepAngleTouchbarSlider.Maximum = FanKit.Math.PiTwice;
            this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPieLayer>
            (
                layerType: LayerType.GeometryPie,
                cache: (tLayer) => tLayer.CacheSweepAngle()
            );
            this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryPieLayer>
            (
                layerType: LayerType.GeometryPie,
                set: (tLayer) => tLayer.SweepAngle = (float)value
            );
            this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingSweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}