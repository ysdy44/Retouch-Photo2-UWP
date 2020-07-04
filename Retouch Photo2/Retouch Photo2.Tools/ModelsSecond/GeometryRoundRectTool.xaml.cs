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
        TipViewModel TipViewModel => App.TipViewModel;
       
        //@TouchBar  
        private bool TouchBarMode
        {
            set
            {
                if (value == false)
                {
                    this.CornerTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarPicker = null;
                    this.TipViewModel.TouchbarSlider = null;
                }
                else
                {
                    this.CornerTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarPicker = this.CornerTouchbarPicker;
                    this.TipViewModel.TouchbarSlider = this.CornerTouchbarSlider;
                }
            }
        }
        
        //@Converter
        private int CornerNumberConverter(float corner) => (int)(corner * 100.0f);
        private double CornerValueConverter(float corner) => corner * 100d;

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
            this.TouchBarMode = false;
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

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryRoundRect");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CornerTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryRoundRect_Corner");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryRoundRectIcon();
        readonly Button _button = new Button { Tag = new GeometryRoundRectIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryRoundRectLayer(customDevice)
            {
                Corner = this.SelectionViewModel.GeometryRoundRectCorner,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {

        //Corner
        private void ConstructCorner1()
        {
            //Button
            this.CornerTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            this.CornerTouchbarPicker.Unit = "%";
            this.CornerTouchbarPicker.Minimum = 0;
            this.CornerTouchbarPicker.Maximum = 50;
            this.CornerTouchbarPicker.ValueChange += (sender, value) =>
            {
                float corner = (float)value / 100.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryRoundRectCorner = corner,
                    set: (tLayer) => tLayer.Corner = corner,

                    historyTitle: "Set round rect layer corner",
                    getHistory: (tLayer) => tLayer.Corner,
                    setHistory: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

        private void ConstructCorner2()
        {
            this.CornerTouchbarSlider.Minimum = 0.0d;
            this.CornerTouchbarSlider.Maximum = 0.5d;
            this.CornerTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryRoundRectLayer>
            (
                layerType: LayerType.GeometryRoundRect,
                cache: (tLayer) => tLayer.CacheCorner()
            );
            this.CornerTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryRoundRectLayer>
            (
                layerType: LayerType.GeometryRoundRect,
                set: (tLayer) => tLayer.Corner = (float)value
            );
            this.CornerTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float corner = (float)value;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryRoundRectCorner = corner,
                    set: (tLayer) => tLayer.Corner = corner,

                    historyTitle: "Set round rect layer corner",
                    getHistory: (tLayer) => tLayer.StartingCorner,
                    setHistory: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

    }
}