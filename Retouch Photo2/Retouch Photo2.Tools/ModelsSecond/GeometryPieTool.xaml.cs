using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : Page, ITool
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
                switch (value)
                {
                    case false:
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.SweepAngleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.SweepAngleTouchbarSlider;
                        break;
                }
            }
        }
        
        //@Converter
        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);
        private double SweepAngleValueConverter(float sweepAngle) => sweepAngle / System.Math.PI * 180d;

        //@Construct
        /// <summary>
        /// Initializes a GeometryPieTool. 
        /// </summary>
        public GeometryPieTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public sealed partial class GeometryPieTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryPie");
            this._button.Style = this.IconSelectedButtonStyle;

            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPie;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryPieIcon();
        readonly Button _button = new Button { Tag = new GeometryPieIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPieLayer(customDevice)
            {
                SweepAngle = this.SelectionViewModel.GeometryPieSweepAngle,
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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : Page, ITool
    {
        //SweepAngle
        private void ConstructSweepAngle1()
        {
            //Button
            this.SweepAngleTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = true;
                else
                    this.TouchBarMode = false;
            };

            //Number
            this.SweepAngleTouchbarSlider.Unit = "º";
            this.SweepAngleTouchbarSlider.NumberMinimum = 0;
            this.SweepAngleTouchbarSlider.NumberMaximum = 360;
            this.SweepAngleTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                
                this.MethodViewModel.TLayerChanged<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.SweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }
        private void ConstructSweepAngle2()
        {
            //Value            
            this.SweepAngleTouchbarSlider.Value = 0;
            this.SweepAngleTouchbarSlider.Minimum = 0;
            this.SweepAngleTouchbarSlider.Maximum = 360;
            this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                this.MethodViewModel.TLayerChangeStarted<GeometryPieLayer>
                (
                    LayerType.GeometryPie,
                    (tLayer) => tLayer.CacheSweepAngle()
                );
            };
            this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;

                this.MethodViewModel.TLayerChangeDelta<GeometryPieLayer>
                (
                    LayerType.GeometryPie,
                    (tLayer) => tLayer.SweepAngle = sweepAngle
                );
            };
            this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryPieLayer>
                (
                    layerType: LayerType.GeometryPie,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle,
                    set: (tLayer) => tLayer.SweepAngle = sweepAngle,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingSweepAngle,
                    setHistory: (tLayer, previous) => tLayer.SweepAngle = previous
                );
            };
        }

    }
}