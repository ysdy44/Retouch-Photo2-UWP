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
    internal enum GeometryCogMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Count. </summary>
        Count,

        /// <summary> Inner-radius. </summary>
        InnerRadius,

        /// <summary> Tooth. </summary>
        Tooth,

        /// <summary> Notch. </summary>
        Notch
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public sealed partial class GeometryCogTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar
        private GeometryCogMode TouchBarMode
        {
            set
            {
                this.CountTouchbarButton.IsSelected = (value == GeometryCogMode.Count);
                this.InnerRadiusTouchbarButton.IsSelected = (value == GeometryCogMode.InnerRadius);
                this.ToothTouchbarButton.IsSelected = (value == GeometryCogMode.Tooth);
                this.NotchTouchbarButton.IsSelected = (value == GeometryCogMode.Notch);

                switch (value)
                {
                    case GeometryCogMode.None:
                        this.TipViewModel.TouchbarPicker = null;
                        this.TipViewModel.TouchbarSlider = null;
                        break;
                    case GeometryCogMode.Count:
                        this.TipViewModel.TouchbarPicker = this.CountTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.CountTouchbarSlider;
                        break;
                    case GeometryCogMode.InnerRadius:
                        this.TipViewModel.TouchbarPicker = this.InnerRadiusTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.InnerRadiusTouchbarSlider;
                        break;
                    case GeometryCogMode.Tooth:
                        this.TipViewModel.TouchbarPicker = this.ToothTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.ToothTouchbarSlider;
                        break;
                    case GeometryCogMode.Notch:
                        this.TipViewModel.TouchbarPicker = this.NotchTouchbarPicker;
                        this.TipViewModel.TouchbarSlider = this.NotchTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter    
        private double CountValueConverter(float count) => count;

        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;

        private int ToothNumberConverter(float tooth) => (int)(tooth * 100.0f);
        private double ToothValueConverter(float tooth) => tooth * 100d;

        private int NotchNumberConverter(float notch) => (int)(notch * 100.0f);
        private double NotchValueConverter(float notch) => notch * 100d;


        //@Construct
        /// <summary>
        /// Initializes a GeometryCogTool. 
        /// </summary>
        public GeometryCogTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCount1();
            this.ConstructCount2();

            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();

            this.ConstructTooth1();
            this.ConstructTooth2();

            this.ConstructNotch1();
            this.ConstructNotch2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryCogMode.None;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public partial class GeometryCogTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryCog");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CountTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Count");
            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_InnerRadius");
            this.ToothTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Tooth");
            this.NotchTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Notch");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCog;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCogIcon();
        readonly Button _button = new Button { Tag = new GeometryCogIcon() };

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryCogLayer(customDevice)
            {
                Count = this.SelectionViewModel.GeometryCogCount,
                InnerRadius = this.SelectionViewModel.GeometryCogInnerRadius,
                Tooth = this.SelectionViewModel.GeometryCogTooth,
                Notch = this.SelectionViewModel.GeometryCogNotch,
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
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public partial class GeometryCogTool : Page, ITool
    {

        //Count
        private void ConstructCount1()
        {
            //Button
            this.CountTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Count;
                else
                    this.TouchBarMode = GeometryCogMode.None;
            };

            this.CountTouchbarPicker.Unit = null;
            this.CountTouchbarPicker.Minimum = 4;
            this.CountTouchbarPicker.Maximum = 36;
            this.CountTouchbarPicker.ValueChange += (sender, value) =>
            {
                int count = (int)value;

                this.MethodViewModel.TLayerChanged<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogCount = count,
                    set: (tLayer) => tLayer.Count = count,

                    historyTitle: "Set cog layer count",
                    getHistory: (tLayer) => tLayer.Count = count,
                    setHistory: (tLayer, previous) => tLayer.Count = previous
                );
            };
        }

        private void ConstructCount2()
        {
            this.CountTouchbarSlider.Minimum = 4;
            this.CountTouchbarSlider.Maximum = 36;
            this.CountTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>
            (
                 layerType: LayerType.GeometryCog,
                 cache: (tLayer) => tLayer.CacheCount()
            );
            this.CountTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>
            (
                 layerType: LayerType.GeometryCog,
                 set: (tLayer) => tLayer.Count = (int)value
            );
            this.CountTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                int count = (int)value;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogCount = count,
                    set: (tLayer) => tLayer.Count = count,

                    historyTitle: "Set cog layer count",
                    getHistory: (tLayer) => tLayer.StartingCount,
                    setHistory: (tLayer, previous) => tLayer.Count = previous
                );
            };
        }


        //InnerRadius
        private void ConstructInnerRadius1()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryCogMode.None;
            };

            this.InnerRadiusTouchbarPicker.Unit = "%";
            this.InnerRadiusTouchbarPicker.Minimum = 0;
            this.InnerRadiusTouchbarPicker.Maximum = 100;
            this.InnerRadiusTouchbarPicker.ValueChange += (sender, value) =>
            {
                float innerRadius = (float)value / 100f;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogInnerRadius = innerRadius,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set cog layer inner radius",
                    getHistory: (tLayer) => tLayer.InnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusTouchbarSlider.Minimum = 0.0d;
            this.InnerRadiusTouchbarSlider.Maximum = 1.0d;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                cache: (tLayer) => tLayer.CacheInnerRadius()
            );
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                set: (tLayer) => tLayer.InnerRadius = (float)value
            );
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogInnerRadius = innerRadius,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set cog layer inner radius",
                    getHistory: (tLayer) => tLayer.StartingInnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }


        //Tooth
        private void ConstructTooth1()
        {
            //Button
            this.ToothTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Tooth;
                else
                    this.TouchBarMode = GeometryCogMode.None;
            };

            this.ToothTouchbarPicker.Unit = "%";
            this.ToothTouchbarPicker.Minimum = 0;
            this.ToothTouchbarPicker.Maximum = 50;
            this.ToothTouchbarPicker.ValueChange += (sender, value) =>
            {
                float tooth = (float)value / 100f;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogTooth = tooth,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    historyTitle: "Set cog layer tooth",
                    getHistory: (tLayer) => tLayer.Tooth,
                    setHistory: (tLayer, previous) => tLayer.Tooth = previous
                );
            };
        }

        private void ConstructTooth2()
        {
            this.ToothTouchbarSlider.Minimum = 0.0d;
            this.ToothTouchbarSlider.Maximum = 0.5d;
            this.ToothTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                cache: (tLayer) => tLayer.CacheTooth()
            );
            this.ToothTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                set: (tLayer) => tLayer.Tooth = (float)value
            );
            this.ToothTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float tooth = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogTooth = tooth,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    historyTitle: "Set cog layer tooth",
                    getHistory: (tLayer) => tLayer.StartingTooth,
                    setHistory: (tLayer, previous) => tLayer.Tooth = previous
                );
            };
        }


        //Notch
        private void ConstructNotch1()
        {
            //Button
            this.NotchTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Notch;
                else
                    this.TouchBarMode = GeometryCogMode.None;
            };

            this.NotchTouchbarPicker.Unit = "%";
            this.NotchTouchbarPicker.Minimum = 0;
            this.NotchTouchbarPicker.Maximum = 60;
            this.NotchTouchbarPicker.ValueChange += (sender, value) =>
            {
                float notch = (float)value / 100f;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogNotch = notch,
                    set: (tLayer) => tLayer.Notch = notch,

                    historyTitle: "Set cog layer notch",
                    getHistory: (tLayer) => tLayer.Notch,
                    setHistory: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

        private void ConstructNotch2()
        {
            this.NotchTouchbarSlider.Minimum = 0.0d;
            this.NotchTouchbarSlider.Maximum = 0.6d;
            this.NotchTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                cache: (tLayer) => tLayer.CacheNotch()
            );
            this.NotchTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>
            (
                layerType: LayerType.GeometryCog,
                set: (tLayer) => tLayer.Notch = (float)value
            );
            this.NotchTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float notch = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryCogNotch = notch,
                    set: (tLayer) => tLayer.Notch = notch,

                    historyTitle: "Set cog layer notch",
                    getHistory: (tLayer) => tLayer.StartingNotch,
                    setHistory: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

    }
}