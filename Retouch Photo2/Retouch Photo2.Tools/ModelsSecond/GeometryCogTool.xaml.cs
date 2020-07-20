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
    /// <see cref="GeometryTool"/>'s GeometryCogTool.
    /// </summary>
    public partial class GeometryCogTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryCog;
        public FrameworkElement Icon { get; } = new GeometryCogIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryCogIcon()
        };
        public FrameworkElement Page { get; } = new GeometryCogPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryCogTool. 
        /// </summary>
        public GeometryCogTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
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


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryCog");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryCogTool"/>.
    /// </summary>
    public partial class GeometryCogPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter    
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private int ToothToNumberConverter(float tooth) => (int)(tooth * 100.0f);
        private int NotchToNumberConverter(float notch) => (int)(notch * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryCogPage. 
        /// </summary>
        public GeometryCogPage()
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

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CountButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Count");
            this.InnerRadiusButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_InnerRadius");
            this.ToothButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Tooth");
            this.NotchButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Notch");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryCogTool"/>.
    /// </summary>
    public partial class GeometryCogPage : Page
    {

        //Count
        private void ConstructCount1()
        {
            this.CountPicker.Unit = null;
            this.CountPicker.Minimum = 4;
            this.CountPicker.Maximum = 36;
            this.CountPicker.ValueChanged += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCogCount = count;

                this.MethodViewModel.TLayerChanged<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Count = count,

                    historyTitle: "Set cog layer count",
                    getHistory: (tLayer) => tLayer.Count,
                    setHistory: (tLayer, previous) => tLayer.Count = previous
                );
            };
        }

        private void ConstructCount2()
        {
            this.CountSlider.Minimum = 4;
            this.CountSlider.Maximum = 36;
            this.CountSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheCount());
            this.CountSlider.ValueChangeDelta += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCogCount = count;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Count = count);
            };
            this.CountSlider.ValueChangeCompleted += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCogCount = count;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
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
            this.InnerRadiusPicker.Unit = "%";
            this.InnerRadiusPicker.Minimum = 0;
            this.InnerRadiusPicker.Maximum = 100;
            this.InnerRadiusPicker.ValueChanged += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCogInnerRadius = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    historyTitle: "Set cog layer inner radius",
                    getHistory: (tLayer) => tLayer.InnerRadius,
                    setHistory: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusSlider.Minimum = 0.0d;
            this.InnerRadiusSlider.Maximum = 1.0d;
            this.InnerRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheInnerRadius());
            this.InnerRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCogInnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCogInnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
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
            this.ToothPicker.Unit = "%";
            this.ToothPicker.Minimum = 0;
            this.ToothPicker.Maximum = 50;
            this.ToothPicker.ValueChanged += (sender, value) =>
            {
                float tooth = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCogTooth = tooth;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    historyTitle: "Set cog layer tooth",
                    getHistory: (tLayer) => tLayer.Tooth,
                    setHistory: (tLayer, previous) => tLayer.Tooth = previous
                );
            };
        }

        private void ConstructTooth2()
        {
            this.ToothSlider.Minimum = 0.0d;
            this.ToothSlider.Maximum = 0.5d;
            this.ToothSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheTooth());
            this.ToothSlider.ValueChangeDelta += (sender, value) =>
            {
                float tooth = (float)value;
                this.SelectionViewModel.GeometryCogTooth = tooth;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Tooth = tooth);
            };
            this.ToothSlider.ValueChangeCompleted += (sender, value) =>
            {
                float tooth = (float)value;
                this.SelectionViewModel.GeometryCogTooth = tooth;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
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
            this.NotchPicker.Unit = "%";
            this.NotchPicker.Minimum = 0;
            this.NotchPicker.Maximum = 60;
            this.NotchPicker.ValueChanged += (sender, value) =>
            {
                float notch = (float)value / 100.0f;
                this.SelectionViewModel.GeometryCogNotch = notch;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    historyTitle: "Set cog layer notch",
                    getHistory: (tLayer) => tLayer.Notch,
                    setHistory: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

        private void ConstructNotch2()
        {
            this.NotchSlider.Minimum = 0.0d;
            this.NotchSlider.Maximum = 0.6d;
            this.NotchSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryCogLayer>(layerType: LayerType.GeometryCog, cache: (tLayer) => tLayer.CacheNotch());
            this.NotchSlider.ValueChangeDelta += (sender, value) =>
            {
                float notch = (float)value;
                this.SelectionViewModel.GeometryCogNotch = notch;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Notch = notch);
            };
            this.NotchSlider.ValueChangeCompleted += (sender, value) =>
            {
                float notch = (float)value;
                this.SelectionViewModel.GeometryCogNotch = notch;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    historyTitle: "Set cog layer notch",
                    getHistory: (tLayer) => tLayer.StartingNotch,
                    setHistory: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

    }
}