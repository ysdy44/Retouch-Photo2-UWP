using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
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


        //@Converter
        private int ValueNumberConverter(float value) => (int)(value * 100.0f);


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
            TouchbarButton.Instance = null;
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

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryArrow");

            this.ValueTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_LeftTail");

            this.RightTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_RightTail");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryArrow; 
        public FrameworkElement Icon { get; } = new GeometryArrowIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryArrowIcon()
        };
        public FrameworkElement Page => this;


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


        public void Started(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => ToolBase.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => ToolBase.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => ToolBase.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : Page, ITool
    {

        //Value
        private void ConstructValue1()
        {
            this.ValueTouchbarPicker.Unit = "%";
            this.ValueTouchbarPicker.Minimum = 0;
            this.ValueTouchbarPicker.Maximum = 100;
            this.ValueTouchbarPicker.ValueChange += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;

                this.MethodViewModel.TLayerChanged<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowValue = value2,
                    set: (tLayer) => tLayer.Value = value2,

                    historyTitle: "Set arrow layer value",
                    getHistory: (tLayer) => tLayer.Value,
                    setHistory: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }

        private void ConstructValue2()
        { 
            this.ValueTouchbarSlider.Minimum = 0.0d;
            this.ValueTouchbarSlider.Maximum = 1.0d;
            this.ValueTouchbarSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryArrowLayer>
            (
                layerType: LayerType.GeometryArrow,
                cache: (tLayer) => tLayer.CacheValue()
            );
            this.ValueTouchbarSlider.ValueChangeDelta += (sender, value) => this.MethodViewModel.TLayerChangeDelta<GeometryArrowLayer>
            (
                layerType: LayerType.GeometryArrow,
                set: (tLayer) => tLayer.Value = (float)value
            );
            this.ValueTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float value2 = (float)value;
                
                this.MethodViewModel.TLayerChangeCompleted<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowValue = value2,
                    set: (tLayer) => tLayer.Value = value2,

                    historyTitle: "Set arrow layer value",
                    getHistory: (tLayer) => tLayer.StartingValue,
                    setHistory: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }

               
        //LeftTail
        private void ConstructLeftTail()
        {
            this.LeftArrowTailTypeComboBox.TypeChanged += (s, tailType) => this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
            (
                layerType: LayerType.GeometryArrow,
                setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowLeftTail = tailType,
                set: (tLayer) => tLayer.LeftTail = tailType,

                historyTitle: "Set arrow layer tail type",
                getHistory: (tLayer) => tLayer.LeftTail,
                setHistory: (tLayer, previous) => tLayer.LeftTail = previous
            );
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightArrowTailTypeComboBox.TypeChanged += (s, tailType) => this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
            (
                layerType: LayerType.GeometryArrow,
                setSelectionViewModel: () => this.SelectionViewModel.GeometryArrowRightTail = tailType,
                set: (tLayer) => tLayer.RightTail = tailType,

                historyTitle: "Set arrow layer tail type",
                getHistory: (tLayer) => tLayer.RightTail,
                setHistory: (tLayer, previous) => tLayer.RightTail = previous
            );
        }

    }
}