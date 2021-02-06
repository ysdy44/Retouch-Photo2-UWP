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
    /// <see cref="GeometryTool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryPie;
        public FrameworkElement Icon { get; } = new GeometryPieIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryPieIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPiePage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryPieTool. 
        /// </summary>
        public GeometryPieTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPieLayer(customDevice)
            {
                SweepAngle = this.SelectionViewModel.GeometryPieSweepAngle,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryPie");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryPieTool"/>.
    /// </summary>
    internal partial class GeometryPiePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int SweepAngleToNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryPiePage. 
        /// </summary>
        public GeometryPiePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SweepAngleButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryPieTool"/>.
    /// </summary>
    internal partial class GeometryPiePage : Page
    {
        //SweepAngle
        private void ConstructSweepAngle1()
        {
            this.SweepAnglePicker.Unit = "º";
            this.SweepAnglePicker.Minimum = 0;
            this.SweepAnglePicker.Maximum = 360;
            this.SweepAnglePicker.ValueChanged += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChanged<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructSweepAngle2()
        {
            this.SweepAngleSlider.Minimum = 0.0d;
            this.SweepAngleSlider.Maximum = FanKit.Math.PiTwice;
            this.SweepAngleSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPieLayer>(layerType: LayerType.GeometryPie, cache: (tLayer) => tLayer.CacheSweepAngle());
            this.SweepAngleSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeDelta<GeometryPieLayer>(layerType: LayerType.GeometryPie, set: (tLayer) => tLayer.SweepAngle = sweepAngle);
            };
            this.SweepAngleSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingSweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}