﻿// Core:              ★★★
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
    /// <see cref="GeometryTool"/>'s GeometryPentagonTool.
    /// </summary>
    public partial class GeometryPentagonTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public FrameworkElement Icon { get; } = new GeometryPentagonIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            Type = ToolType.GeometryPentagon,
            CenterContent = new GeometryPentagonIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPentagonPage();


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryPentagonLayer
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
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
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChanged<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryPentagonLayer_Points,
                    getUndo: (tLayer) => tLayer.Points,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
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
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChangeDelta<GeometryPentagonLayer>(layerType: LayerType.GeometryPentagon, set: (tLayer) => tLayer.Points = points);
            };
            this.PointsSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryPentagonLayer_Points,
                    getUndo: (tLayer) => tLayer.StartingPoints,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

    }
}