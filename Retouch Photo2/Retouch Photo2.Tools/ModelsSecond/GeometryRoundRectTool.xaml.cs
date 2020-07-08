using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public partial class GeometryRoundRectTool : Page, ITool
    {  

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
       
        
        //@Converter
        private int CornerToNumberConverter(float corner) => (int)(corner * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryRoundRectTool. 
        /// </summary>
        public GeometryRoundRectTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCorner1();
            this.ConstructCorner2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryRoundRect");

            this.CornerButton.CenterContent = resource.GetString("/ToolsSecond/GeometryRoundRect_Corner");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
        public FrameworkElement Icon { get; } = new GeometryRoundRectIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryRoundRectIcon()
        };
        public FrameworkElement Page => this;


        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryRoundRectLayer(customDevice)
            {
                Corner = this.SelectionViewModel.GeometryRoundRectCorner,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {

        //Corner
        private void ConstructCorner1()
        {
            this.CornerPicker.Unit = "%";
            this.CornerPicker.Minimum = 0;
            this.CornerPicker.Maximum = 50;
            this.CornerPicker.ValueChange += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                this.SelectionViewModel.GeometryRoundRectCorner = corner;

                this.MethodViewModel.TLayerChanged<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    historyTitle: "Set round rect layer corner",
                    getHistory: (tLayer) => tLayer.Corner,
                    setHistory: (tLayer, previous) => tLayer.Corner = previous
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
                this.SelectionViewModel.GeometryRoundRectCorner = corner;

                this.MethodViewModel.TLayerChangeDelta<GeometryRoundRectLayer>(layerType: LayerType.GeometryRoundRect, set: (tLayer) => tLayer.Corner = corner);
            };
            this.CornerSlider.ValueChangeCompleted += (sender, value) =>
            {
                float corner = (float)value;
                this.SelectionViewModel.GeometryRoundRectCorner = corner;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    historyTitle: "Set round rect layer corner",
                    getHistory: (tLayer) => tLayer.StartingCorner,
                    setHistory: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

    }
}