// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
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
    /// <see cref="GeometryTool"/>'s GeometryCookieTool.
    /// </summary>
    public partial class GeometryCookieTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryCookie;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title => this.GeometryCookiePage.Title;
        public ControlTemplate Icon => this.GeometryCookiePage.Icon;
        public FrameworkElement Page => this.GeometryCookiePage;
        readonly GeometryCookiePage GeometryCookiePage = new GeometryCookiePage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryCookiePage.IsOpen; set => this.GeometryCookiePage.IsOpen = value; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCookieLayer
            {
                InnerRadius = this.SelectionViewModel.GeometryCookie_InnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryCookie_SweepAngle,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryCookieTool"/>.
    /// </summary>
    internal partial class GeometryCookiePage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private int SweepAngleToNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryCookiePage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryCookiePage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryCookiePage), new PropertyMetadata(false));


        #endregion


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a GeometryCookiePage. 
        /// </summary>
        public GeometryCookiePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();

            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();

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

            this.Title = resource.GetString("Tools_GeometryCookie");

            this.InnerRadiusTextBlock.Text = resource.GetString("Tools_GeometryCookie_InnerRadius");
            this.SweepAngleTextBlock.Text = resource.GetString("Tools_GeometryCookie_SweepAngle");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryCookieTool"/>.
    /// </summary>
    internal partial class GeometryCookiePage : Page
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
                this.SelectionViewModel.GeometryCookie_SweepAngle = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.SweepAngle = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCookieLayer_SweepAngle,
                    getUndo: (tLayer) => tLayer.SweepAngle,
                    setUndo: (tLayer, previous) => tLayer.SweepAngle = previous
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
                this.SelectionViewModel.GeometryCookie_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryCookie_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryCookieLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.StartingInnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
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
                this.SelectionViewModel.GeometryCookie_InnerRadius = sweepAngle;

                this.MethodViewModel.TLayerChanged<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.InnerRadius = sweepAngle,

                    type: HistoryType.LayersProperty_Set_GeometryCookieLayer_SweepAngle,
                    getUndo: (tLayer) => tLayer.SweepAngle,
                    setUndo: (tLayer, previous) => tLayer.SweepAngle = previous
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
                this.SelectionViewModel.GeometryCookie_SweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeDelta<GeometryCookieLayer>(layerType: LayerType.GeometryCookie, set: (tLayer) => tLayer.SweepAngle = sweepAngle);
            };
            this.SweepAngleSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value;
                this.SelectionViewModel.GeometryCookie_SweepAngle = sweepAngle;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryCookieLayer>
                (
                    layerType: LayerType.GeometryCookie,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    type: HistoryType.LayersProperty_Set_GeometryCookieLayer_SweepAngle,
                    getUndo: (tLayer) => tLayer.StartingSweepAngle,
                    setUndo: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}