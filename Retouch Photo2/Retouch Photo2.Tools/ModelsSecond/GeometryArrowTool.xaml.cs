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
    internal enum GeometryArrowMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Width (IsAbsolute = false). </summary>
        Width,

        /// <summary> Value (IsAbsolute = false). </summary>
        Value
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public sealed partial class GeometryArrowTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar
        private GeometryArrowMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryArrowMode.None:
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryArrowMode.Width:
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryArrowMode.Value:
                        this.ValueTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.ValueTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int ValueNumberConverter(float value) => (int)(value * 100.0f);
        private double ValueValueConverter(float value) => value * 100d;


        //@Construct
        /// <summary>
        /// Initializes a GeometryArrowTool. 
        /// </summary>
        public GeometryArrowTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructValue1();
            this.ConstructValue2();

            this.ConstructLeftTail();
            this.ConstructRightTail();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryArrowMode.None;
        }        
    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = 
                this.Title = resource.GetString("/ToolsSecond/GeometryArrow");
            this._button.Style = this.IconSelectedButtonStyle;

            this.ValueTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_LeftTail");

            this.RightTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_RightTail");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryArrow; 
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryArrowIcon();
        readonly Button _button = new Button { Tag = new GeometryArrowIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryArrowLayer(customDevice)
            {
                LeftTail = this.SelectionViewModel.GeometryArrowLeftTail,
                RightTail = this.SelectionViewModel.GeometryArrowRightTail,
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
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : Page, ITool
    {

        //Value
        private void ConstructValue1()
        {
            //Button
            this.ValueTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryArrowMode.Value;
                else
                    this.TouchBarMode = GeometryArrowMode.None;
            };

            //Number
            this.ValueTouchbarSlider.Unit = "%";
            this.ValueTouchbarSlider.NumberMinimum = 0;
            this.ValueTouchbarSlider.NumberMaximum = 100;
            this.ValueTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;
                if (value2 < 0.0f) value2 = 0.0f;
                if (value2 > 1.0f) value2 = 1.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowValue = value2,
                    set: (tLayer) => tLayer.Value = value2,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.Value,
                    setHistory: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }
        private void ConstructValue2()
        { 
            //Value
            this.ValueTouchbarSlider.Value = 0;
            this.ValueTouchbarSlider.Minimum = 0;
            this.ValueTouchbarSlider.Maximum = 100;
            this.ValueTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                this.MethodViewModel.TLayerChangeStarted<GeometryArrowLayer>
                (
                    LayerType.GeometryArrow,
                    (tLayer) => tLayer.CacheValue()
                );
            };
            this.ValueTouchbarSlider.ValueChangeDelta += (sender, value2) =>
            {
                float value3 = (float)value2 / 100.0f;
                if (value3 < 0.0f) value3 = 0.0f;
                if (value3 > 1.0f) value3 = 1.0f;

                this.MethodViewModel.TLayerChangeDelta<GeometryArrowLayer>
                (
                    LayerType.GeometryArrow,
                    (tLayer) => tLayer.Value = value3
                );
            };
            this.ValueTouchbarSlider.ValueChangeCompleted += (sender, value2) =>
            {
                float value3 = (float)value2 / 100.0f;
                if (value3 < 0.0f) value3 = 0.0f;
                if (value3 > 1.0f) value3 = 1.0f;
                
                this.MethodViewModel.TLayerChangeCompleted<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowValue = value3,
                    set: (tLayer) => tLayer.Value = value3,

                    historyTitle: "Set pie layer sweep angle",
                    getHistory: (tLayer) => tLayer.StartingValue,
                    setHistory: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }

               
        //LeftTail
        private void ConstructLeftTail()
        {
            this.LeftArrowTailTypeComboBox.TypeChanged += (s, tailType) =>
            {
                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowLeftTail = tailType,
                    set: (tLayer) => tLayer.LeftTail = tailType,

                    historyTitle: "Set arrow layer tail type",
                    getHistory: (tLayer) => tLayer.LeftTail,
                    setHistory: (tLayer, previous) => tLayer.LeftTail = previous
                );
            };
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightArrowTailTypeComboBox.TypeChanged += (s, tailType) =>
            {
                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowRightTail = tailType,
                    set: (tLayer) => tLayer.RightTail = tailType,

                    historyTitle: "Set arrow layer tail type",
                    getHistory: (tLayer) => tLayer.RightTail,
                    setHistory: (tLayer, previous) => tLayer.RightTail = previous
                );
            };
        }

    }
}