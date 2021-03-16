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
    internal enum GeometryStarMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Points. </summary>
        Points,

        /// <summary> Inner-radius. </summary>
        InnerRadius,
    }

    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryStarTool.
    /// </summary>
    public partial class GeometryStarTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryStar;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page => this.GeometryStarPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryStarPage.IsOpen; set => this.GeometryStarPage.IsOpen = value; }
        readonly GeometryStarPage GeometryStarPage = new GeometryStarPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryStarLayer
            {
                Points = this.SelectionViewModel.GeometryStar_Points,
                InnerRadius = this.SelectionViewModel.GeometryStar_InnerRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryStarTool"/>.
    /// </summary>
    public partial class GeometryStarPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private int InnerRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryStarPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryStarPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryStarPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryStarPage. 
        /// </summary>
        public GeometryStarPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructPoints1();
            this.ConstructPoints2();
            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();

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

            this.PointsTextBlock.Text = resource.GetString("Tools_GeometryStar_Points");
            this.InnerRadiusTextBlock.Text = resource.GetString("Tools_GeometryStar_InnerRadius");
        }
    }


    public partial class GeometryStarPage : Page
    {

        //Points
        private void ConstructPoints1()
        {
            this.PointsPicker.Unit = null;
            this.PointsPicker.Minimum = 3;
            this.PointsPicker.Maximum = 36;
            this.PointsPicker.ValueChanged += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryStar_Points = points;

                this.MethodViewModel.TLayerChanged<int, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryStarLayer_Points,
                    getUndo: (tLayer) => tLayer.Points,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

        private void ConstructPoints2()
        {
            this.PointsSlider.Minimum = 3.0d;
            this.PointsSlider.Maximum = 36.0d;
            this.PointsSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryStarLayer>(layerType: LayerType.GeometryStar, cache: (tLayer) => tLayer.CachePoints());
            this.PointsSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryStar_Points = points;

                this.MethodViewModel.TLayerChangeDelta<GeometryStarLayer>(layerType: LayerType.GeometryStar, set: (tLayer) => tLayer.Points = points);
            };
            this.PointsSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryStar_Points = points;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryStarLayer_Points,
                    getUndo: (tLayer) => tLayer.StartingPoints,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
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
                this.SelectionViewModel.GeometryStar_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryStarLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.InnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

        private void ConstructInnerRadius2()
        {
            this.InnerRadiusSlider.Minimum = 0.0d;
            this.InnerRadiusSlider.Maximum = 1.0d;
            this.InnerRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryStarLayer>(layerType: LayerType.GeometryStar, cache: (tLayer) => tLayer.CacheInnerRadius());
            this.InnerRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryStar_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryStarLayer>(layerType: LayerType.GeometryStar, set: (tLayer) => tLayer.InnerRadius = innerRadius);
            };
            this.InnerRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value;
                this.SelectionViewModel.GeometryStar_InnerRadius = innerRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryStarLayer>
                (
                    layerType: LayerType.GeometryStar,
                    set: (tLayer) => tLayer.InnerRadius = innerRadius,

                    type: HistoryType.LayersProperty_Set_GeometryStarLayer_InnerRadius,
                    getUndo: (tLayer) => tLayer.StartingInnerRadius,
                    setUndo: (tLayer, previous) => tLayer.InnerRadius = previous
                );
            };
        }

    }
}