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
    /// <see cref="GeometryTool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : GeometryTool, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryArrow;
        public FrameworkElement Icon { get; } = new GeometryArrowIcon();
        public IToolButton Button { get; } = new ToolSecondButton
        {
            CenterContent = new GeometryArrowIcon()
        };
        public FrameworkElement Page { get; } = new GeometryArrowPage();


        //@Construct
        /// <summary>
        /// Initializes a GeometryArrowTool. 
        /// </summary>
        public GeometryArrowTool()
        {
            this.ConstructStrings();
        }


        public override ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryArrowLayer(customDevice)
            {
                LeftTail = this.SelectionViewModel.GeometryArrowLeftTail,
                RightTail = this.SelectionViewModel.GeometryArrowRightTail,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/ToolsSecond/GeometryArrow");
        }
            
    }


    /// <summary>
    /// Page of <see cref="GeometryArrowTool"/>.
    /// </summary>
    internal partial class GeometryArrowPage : Page
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Converter
        private int ValueToNumberConverter(float value) => (int)(value * 100.0f);


        //@Construct
        /// <summary>
        /// Initializes a GeometryArrowPage. 
        /// </summary>
        public GeometryArrowPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructValue1();
            this.ConstructValue2();

            this.ConstructLeftTail();
            this.ConstructRightTail();
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ValueButton.CenterContent = resource.GetString("/ToolsSecond/GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_LeftTail");

            this.RightTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_RightTail");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }
    }

    /// <summary>
    /// Page of <see cref="GeometryArrowTool"/>.
    /// </summary>
    internal partial class GeometryArrowPage : Page
    {

        //Value
        private void ConstructValue1()
        {
            this.ValuePicker.Unit = "%";
            this.ValuePicker.Minimum = 0;
            this.ValuePicker.Maximum = 100;
            this.ValuePicker.ValueChanged += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;
                this.SelectionViewModel.GeometryArrowValue = value2;

                this.MethodViewModel.TLayerChanged<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.Value = value2,

                    historyTitle: "Set arrow layer value",
                    getHistory: (tLayer) => tLayer.Value,
                    setHistory: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }

        private void ConstructValue2()
        {
            this.ValueSlider.Minimum = 0.0d;
            this.ValueSlider.Maximum = 1.0d;
            this.ValueSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryArrowLayer>(layerType: LayerType.GeometryArrow, cache: (tLayer) => tLayer.CacheValue());
            this.ValueSlider.ValueChangeDelta += (sender, value) =>
            {
                float value2 = (float)value;
                this.SelectionViewModel.GeometryArrowValue = value2;

                this.MethodViewModel.TLayerChangeDelta<GeometryArrowLayer>(layerType: LayerType.GeometryArrow, set: (tLayer) => tLayer.Value = value2);
            };
            this.ValueSlider.ValueChangeCompleted += (sender, value) =>
            {
                float value2 = (float)value;
                this.SelectionViewModel.GeometryArrowValue = value2;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
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
            this.LeftArrowTailTypeComboBox.TypeChanged += (s, type) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)type;
                this.SelectionViewModel.GeometryArrowLeftTail = tailType;

                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.LeftTail = tailType,

                    historyTitle: "Set arrow layer left tail type",
                    getHistory: (tLayer) => tLayer.LeftTail,
                    setHistory: (tLayer, previous) => tLayer.LeftTail = previous
                );
            };
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightArrowTailTypeComboBox.TypeChanged += (s, type) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)type;
                this.SelectionViewModel.GeometryArrowRightTail = tailType;

                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.RightTail = tailType,

                    historyTitle: "Set arrow layer right tail type",
                    getHistory: (tLayer) => tLayer.RightTail,
                    setHistory: (tLayer, previous) => tLayer.RightTail = previous
                );
            };
        }

    }
}