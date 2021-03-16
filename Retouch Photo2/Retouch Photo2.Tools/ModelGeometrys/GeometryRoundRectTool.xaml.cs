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
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryRoundRectTool.
    /// </summary>
    public partial class GeometryRoundRectTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page => this.GeometryRoundRectPage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.GeometryRoundRectPage.IsOpen; set => this.GeometryRoundRectPage.IsOpen = value; }
        readonly GeometryRoundRectPage GeometryRoundRectPage = new GeometryRoundRectPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryRoundRectLayer
            {
                Corner = this.SelectionViewModel.GeometryRoundRect_Corner,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryRoundRectTool"/>.
    /// </summary>
    public partial class GeometryRoundRectPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter
        private int CornerToNumberConverter(float corner) => (int)(corner * 100.0f);


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryRoundRectPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryRoundRectPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryRoundRectPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryRoundRectPage. 
        /// </summary>
        public GeometryRoundRectPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCorner1();
            this.ConstructCorner2();

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

            this.CornerTextBlock.Text = resource.GetString("Tools_GeometryRoundRect_Corner");
            
            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }
    }


    public partial class GeometryRoundRectPage : Page
    {

        //Corner
        private void ConstructCorner1()
        {
            this.CornerPicker.Unit = "%";
            this.CornerPicker.Minimum = 0;
            this.CornerPicker.Maximum = 50;
            this.CornerPicker.ValueChanged += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChanged<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    type: HistoryType.LayersProperty_Set_GeometryRoundRectLayer_Corner,
                    getUndo: (tLayer) => tLayer.Corner,
                    setUndo: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

        private void ConstructCorner2()
        {
            this.CornerSlider.Minimum = 0.0d;
            this.CornerSlider.Maximum = 0.5d;
            this.CornerSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryRoundRectLayer>(layerType: LayerType.GeometryRoundRect, cache: (tLayer) => tLayer.CacheCorner());
            this.CornerSlider.ValueChangeDelta += (sender, value) =>
            {
                float corner = (float)value;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChangeDelta<GeometryRoundRectLayer>(layerType: LayerType.GeometryRoundRect, set: (tLayer) => tLayer.Corner = corner);
            };
            this.CornerSlider.ValueChangeCompleted += (sender, value) =>
            {
                float corner = (float)value;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    type: HistoryType.LayersProperty_Set_GeometryRoundRectLayer_Corner,
                    getUndo: (tLayer) => tLayer.StartingCorner,
                    setUndo: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

    }
}