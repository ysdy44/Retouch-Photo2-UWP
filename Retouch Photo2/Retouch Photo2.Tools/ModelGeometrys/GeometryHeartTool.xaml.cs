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
    /// <see cref="GeometryTool"/>'s GeometryHeartTool.
    /// </summary>
    public partial class GeometryHeartTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryHeart;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new GeometryHeartPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryHeartLayer
            {
                Spread = this.SelectionViewModel.GeometryHeart_Spread,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryHeartTool"/>.
    /// </summary>
    public partial class GeometryHeartPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int SpreadToNumberConverter(float spread) => (int)(spread * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryHeartPage. 
        /// </summary>
        public GeometryHeartPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSpread1();
            this.ConstructSpread2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SpreadTextBlock.Text = resource.GetString("Tools_GeometryHeart_Spread");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryHeartTool"/>.
    /// </summary>
    public partial class GeometryHeartPage : Page
    {

        //Spead
        private void ConstructSpread1()
        {
            this.SpreadPicker.Unit = "%";
            this.SpreadPicker.Minimum = 0;
            this.SpreadPicker.Maximum = 100;
            this.SpreadPicker.ValueChanged += (sender, value) =>
            {
                float spread = (float)value / 100.0f;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChanged<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    set: (tLayer) => tLayer.Spread = spread,

                    type: HistoryType.LayersProperty_Set_GeometryHeartLayer_Spread,
                    getUndo: (tLayer) => tLayer.Spread,
                    setUndo: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

        private void ConstructSpread2()
        {
            this.SpreadSlider.Minimum = 0.0d;
            this.SpreadSlider.Maximum = 1.0d;
            this.SpreadSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryHeartLayer>(layerType: LayerType.GeometryHeart, cache: (tLayer) => tLayer.CacheSpread());
            this.SpreadSlider.ValueChangeDelta += (sender, value) =>
            {
                float spread = (float)value;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChangeDelta<GeometryHeartLayer>(layerType: LayerType.GeometryHeart, set: (tLayer) => tLayer.Spread = spread);
            };
            this.SpreadSlider.ValueChangeCompleted += (sender, value) =>
            {
                float spread = (float)value;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    set: (tLayer) => tLayer.Spread = spread,

                    type: HistoryType.LayersProperty_Set_GeometryHeartLayer_Spread,
                    getUndo: (tLayer) => tLayer.StartingSpread,
                    setUndo: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

    }
}