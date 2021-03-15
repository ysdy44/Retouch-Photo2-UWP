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
        public FrameworkElement Page { get; } = new GeometryRoundRectPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


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


        //@Converter
        private int CornerToNumberConverter(float corner) => (int)(corner * 100.0f);


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

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CornerTextBlock.Text = resource.GetString("Tools_GeometryRoundRect_Corner");
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