// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
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
    /// <summary>
    /// <see cref="GeometryTool"/>'s GeometryPentagonTool.
    /// </summary>
    public partial class GeometryPentagonTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryPentagon;
        public FrameworkElement Icon { get; } = new GeometryPentagonIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryPentagonIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPentagonPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryPentagonTool. 
        /// </summary>
        public GeometryPentagonTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPentagonLayer(customDevice)
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("Tools_GeometryPentagon");
        }

    }


    /// <summary>
    /// Page of <see cref="GeometryPentagonTool"/>.
    /// </summary>
    internal partial class GeometryPentagonPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Construct
        /// <summary>
        /// Initializes a GeometryPentagonPage. 
        /// </summary>
        public GeometryPentagonPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructPoints1();
            this.ConstructPoints2();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.PointsButton.CenterContent = resource.GetString("Tools_GeometryPentagon_Points");

            this.ConvertTextBlock.Text = resource.GetString("Tools_ConvertToCurves");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryPentagonTool"/>.
    /// </summary>
    internal partial class GeometryPentagonPage : Page
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
                this.SelectionViewModel.GeometryPentagonPoints = points;

                this.MethodViewModel.TLayerChanged<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set pentagon layer points",
                    getHistory: (tLayer) => tLayer.Points,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

        private void ConstructPoints2()
        {
            this.PointsSlider.Minimum = 3;
            this.PointsSlider.Maximum = 36;
            this.PointsSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPentagonLayer>(layerType: LayerType.GeometryPentagon, cache: (tLayer) => tLayer.CachePoints());
            this.PointsSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagonPoints = points;

                this.MethodViewModel.TLayerChangeDelta<GeometryPentagonLayer>(layerType: LayerType.GeometryPentagon, set: (tLayer) => tLayer.Points = points);
            };
            this.PointsSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagonPoints = points;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    historyTitle: "Set pentagon layer points",
                    getHistory: (tLayer) => tLayer.StartingPoints,
                    setHistory: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

    }
}