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
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        private GeometryCookieMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryCookieMode.None:
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarPicker = null;
                        this.TipViewModel.TouchbarSlider = null;
                        break;
                    case GeometryCookieMode.InnerRadius:
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarPicker = this.InnerRadiusTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.InnerRadiusTouchbarSlider;
                        break;
                    case GeometryCookieMode.SweepAngle:
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.SweepAngleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarPicker = this.SweepAngleTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.SweepAngleTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;

        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);
        private double SweepAngleValueConverter(float sweepAngle) => sweepAngle / System.Math.PI * 180d;


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
            this.TouchBarMode = GeometryCookieMode.None;
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

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryCookie");
            this._button.Style = this.IconSelectedButtonStyle;

            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_InnerRadius");
            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCookie;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCookieIcon();
        readonly Button _button = new Button { Tag = new GeometryCookieIcon()};
        
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


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public sealed partial class GeometryCookieTool : Page, ITool
    {

        //InnerRadius
        private void ConstructInnerRadius1()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCookieMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryCookieMode.None;
            };

            //Number
            this.InnerRadiusTouchbarPicker.Unit = "%";
            this.InnerRadiusTouchbarPicker.Minimum = 0;
            this.InnerRadiusTouchbarPicker.Maximum = 100;
            this.InnerRadiusTouchbarPicker.ValueChange += (sender, value) =>
            {
                float innerRadius = (float)value / 100f;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCookieSweepAngle = innerRadius,
                    set: (tLayer) => tLayer.SweepAngle = innerRadius,

                    historyTitle: "Set cookie layer inner radius",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusTouchbarSlider.Minimum = 0.0f;
            this.InnerRadiusTouchbarSlider.Maximum = 1.0f;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCookieLayer>
            (
                layerType: LayerType.GeometryCookie,
                cache: (tLayer) => tLayer.CacheInnerRadius()
            );
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>
            (
                layerType: LayerType.GeometryCookie,
                set: (tLayer) => tLayer.InnerRadius = (float)value
            );
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius,
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
            //Button
            this.SweepAngleTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCookieMode.SweepAngle;
                else
                    this.TouchBarMode = GeometryCookieMode.None;
            };

            this.SweepAngleTouchbarPicker.Unit = "º";
            this.SweepAngleTouchbarPicker.Minimum = 0;
            this.SweepAngleTouchbarPicker.Maximum = 360;
            this.SweepAngleTouchbarPicker.ValueChange += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCookieInnerRadius = sweepAngle,
                    set: (tLayer) => tLayer.InnerRadius = sweepAngle,

                    historyTitle: "Set cookie layer sweep angle",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

        private void ConstructSweepAngle2()
        {
            this.SweepAngleTouchbarSlider.Minimum = 0;
            this.SweepAngleTouchbarSlider.Maximum = FanKit.Math.PiTwice;
            this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCookieLayer>
            (
                layerType: LayerType.GeometryCookie,
                cache: (tLayer) => tLayer.CacheSweepAngle()
            );
            this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>
            (
                layerType: LayerType.GeometryCookie,
                set: (tLayer) => tLayer.SweepAngle = (float)value
            );
            this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set cookie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingSweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}