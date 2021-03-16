// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Windows.UI.Xaml.Controls;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryCog;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page => this.GeometryCogPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryCogPage.IsOpen; set => this.GeometryCogPage.IsOpen = value; }
        readonly GeometryCogPage GeometryCogPage = new GeometryCogPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCogLayer
            {
                Count = this.SelectionViewModel.GeometryCog_Count,
                InnerRadius = this.SelectionViewModel.GeometryCog_InnerRadius,
                Tooth = this.SelectionViewModel.GeometryCog_Tooth,
                Notch = this.SelectionViewModel.GeometryCog_Notch,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
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
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter    
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private int ToothToNumberConverter(float tooth) => (int)(tooth * 100.0f);
        private int NotchToNumberConverter(float notch) => (int)(notch * 100.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryCogPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryCogPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryCogPage), new PropertyMetadata(false));


        #endregion


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

            this.ConvertToCurvesButton.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.TipViewModel.ToolType = ToolType.Node;
            };

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CountTextBlock.Text = resource.GetString("Tools_GeometryCog_Count");
            this.InnerRadiusTextBlock.Text = resource.GetString("Tools_GeometryCog_InnerRadius");
            this.ToothTextBlock.Text = resource.GetString("Tools_GeometryCog_Tooth");
            this.NotchTextBlock.Text = resource.GetString("Tools_GeometryCog_Notch");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
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
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChanged<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Count = count,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Count,
                    getUndo: (tLayer) => tLayer.Count,
                    setUndo: (tLayer, previous) => tLayer.Count = previous
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
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Count = count);
            };
            this.CountSlider.ValueChangeCompleted += (sender, value) =>
            {
                int count = (int)value;
                this.SelectionViewModel.GeometryCog_Count = count;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Count = count,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Count,
                    getUndo: (tLayer) => tLayer.StartingCount,
                    setUndo: (tLayer, previous) => tLayer.Count = previous
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
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.InnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
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
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCog_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.StartingInnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
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
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Tooth,
                    getUndo: (tLayer) => tLayer.Tooth,
                    setUndo: (tLayer, previous) => tLayer.Tooth = previous
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
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Tooth = tooth);
            };
            this.ToothSlider.ValueChangeCompleted += (sender, value) =>
            {
                float tooth = (float)value;
                this.SelectionViewModel.GeometryCog_Tooth = tooth;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Tooth = tooth,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Tooth,
                    getUndo: (tLayer) => tLayer.StartingTooth,
                    setUndo: (tLayer, previous) => tLayer.Tooth = previous
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
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChanged<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Notch,
                    getUndo: (tLayer) => tLayer.Notch,
                    setUndo: (tLayer, previous) => tLayer.Notch = previous
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
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChangeDelta<GeometryCogLayer>(layerType: LayerType.GeometryCog, set: (tLayer) => tLayer.Notch = notch);
            };
            this.NotchSlider.ValueChangeCompleted += (sender, value) =>
            {
                float notch = (float)value;
                this.SelectionViewModel.GeometryCog_Notch = notch;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCogLayer>
                (
                    layerType: LayerType.GeometryCog,
                    set: (tLayer) => tLayer.Notch = notch,

                    type: HistoryType.LayersProperty_Set_GeometryCogLayer_Notch,
                    getUndo: (tLayer) => tLayer.StartingNotch,
                    setUndo: (tLayer, previous) => tLayer.Notch = previous
                );
            };
        }

    }
}