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
    /// <see cref="GeometryTool"/>'s GeometryDiamondTool.
    /// </summary>
    public partial class GeometryDiamondTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryDiamond;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new GeometryDiamondPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryDiamondLayer
            {
                Mid = this.SelectionViewModel.GeometryDiamond_Mid,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryDiamondTool"/>.
    /// </summary>
    public partial class GeometryDiamondPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private int MidToNumberConverter(float mid) => (int)(mid * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryDiamondPage. 
        /// </summary>
        public GeometryDiamondPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructMid1();
            this.ConstructMid2();
            this.ConstructMirror();

            this.MoreCreateButton.Click += (s, e) =>
            {
                this.SettingViewModel.ShowFlyout(DrawPage.MoreCreateFlyout, DrawPage.MoreCreateContent, this, this.MoreCreateButton);
            };
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.MidTextBlock.Text = resource.GetString("Tools_GeometryDiamond_Mirror");
            this.MirrorTextBlock.Text = resource.GetString("Tools_GeometryDiamond_Mid");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryDiamondTool"/>.
    /// </summary>
    public partial class GeometryDiamondPage : Page
    {

        //Mid
        private void ConstructMid1()
        {
            this.MidPicker.Unit = "%";
            this.MidPicker.Minimum = 0;
            this.MidPicker.Maximum = 100;
            this.MidPicker.ValueChanged += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.Mid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMid2()
        {
            this.MidSlider.Minimum = 0.0d;
            this.MidSlider.Maximum = 1.0d;
            this.MidSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, cache: (tLayer) => tLayer.CacheMid());
            this.MidSlider.ValueChangeDelta += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChangeDelta<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, set: (tLayer) => tLayer.Mid = mid);
            };
            this.MidSlider.ValueChangeCompleted += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.StartingMid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float mid = 1.0f - this.SelectionViewModel.GeometryDiamond_Mid;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = 1.0f - tLayer.Mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.Mid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

    }
}