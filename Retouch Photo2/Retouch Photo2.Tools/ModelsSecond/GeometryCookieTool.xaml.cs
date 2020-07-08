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
    internal enum GeometryCookieMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Inner-radius. </summary>
        InnerRadius,

        /// <summary> Sweep-angle. </summary>
        SweepAngle
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public partial class GeometryCookieTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        

        //@Converter
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private int SweepAngleToNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryCookieTool. 
        /// </summary>
        public GeometryCookieTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();

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
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public sealed partial class GeometryCookieTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryCookie");

            this.InnerRadiusButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_InnerRadius");
            this.SweepAngleButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCookie;
        public FrameworkElement Icon { get; } = new GeometryCookieIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryCookieIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryCookieLayer(customDevice)
            {
                InnerRadius = this.SelectionViewModel.GeometryCookieInnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryCookieSweepAngle,
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
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public sealed partial class GeometryCookieTool : Page, ITool
    {

        //InnerRadius
        private void ConstructInnerRadius1()
        {
            this.InnerRadiusPicker.Unit = "%";
            this.InnerRadiusPicker.Minimum = 0;
            this.InnerRadiusPicker.Maximum = 100;
            this.InnerRadiusPicker.ValueChanged += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCookieSweepAngle = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.SweepAngle = innerRadius,

                    historyTitle: "Set cookie layer inner radius",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusSlider.Minimum = 0.0d;
            this.InnerRadiusSlider.Maximum = 1.0d;
            this.InnerRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, cache: (tLayer) => tLayer.CacheInnerRadius());
            this.InnerRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set cookie layer inner radius",
                    getHistory: (tLayer) => tLayer.StartingInnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }


        //SweepAngle
        private void ConstructSweepAngle1()
        {
            this.SweepAnglePicker.Unit = "º";
            this.SweepAnglePicker.Minimum = 0;
            this.SweepAnglePicker.Maximum = 360;
            this.SweepAnglePicker.ValueChanged += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                this.SelectionViewModel.GeometryCookieInnerRadius = sweepAngle;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.InnerRadius = sweepAngle,

                    historyTitle: "Set cookie layer sweep angle",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructSweepAngle2()
        {
            this.SweepAngleSlider.Minimum = 0.0d;
            this.SweepAngleSlider.Maximum = FanKit.Math.PiTwice;
            this.SweepAngleSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, cache: (tLayer) => tLayer.CacheSweepAngle());
            this.SweepAngleSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;
                
                this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, set: (tLayer) => tLayer.SweepAngle = sweepAngle);
            };
            this.SweepAngleSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set cookie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingSweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}