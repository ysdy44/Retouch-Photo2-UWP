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
using Retouch_Photo2.Tools.Icons;
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
        public FrameworkElement Icon { get; } = new GeometryRoundRectIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryRoundRectIcon()
        };
        public FrameworkElement Page { get; } = new GeometryRoundRectPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryRoundRectTool. 
        /// </summary>
        public GeometryRoundRectTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryRoundRectLayer(customDevice)
            {
                Corner = this.SelectionViewModel.GeometryRoundRect_Corner,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("Tools_GeometryRoundRect");
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
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CornerButton.CenterContent = resource.GetString("Tools_GeometryRoundRect_Corner");
            this.ConvertTextBlock.Text = resource.GetString("Tools_ConvertToCurves");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryRoundRectTool"/>.
    /// </summary>
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