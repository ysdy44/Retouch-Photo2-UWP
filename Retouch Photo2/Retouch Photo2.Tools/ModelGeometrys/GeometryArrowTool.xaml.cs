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
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public ToolType Type => ToolType.GeometryArrow;
        public ToolGroupType GroupType => ToolGroupType.Geometry;
        public string Title { get; set; }
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Page { get; } = new GeometryArrowPage();
        public bool IsSelected { get; set; }
        public bool IsOpen { get; set; }


        public override ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryArrowLayer
            {
                LeftTail = this.SelectionViewModel.GeometryArrow_LeftTail,
                RightTail = this.SelectionViewModel.GeometryArrow_RightTail,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
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

            this.ValueButton.Content = resource.GetString("Tools_GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("Tools_GeometryArrow_LeftTail");
            this.RightTailTextBlock.Text = resource.GetString("Tools_GeometryArrow_RightTail");
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
                this.SelectionViewModel.GeometryArrow_Value = value2;

                this.MethodViewModel.TLayerChanged<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.Value = value2,

                    type: HistoryType.LayersProperty_Set_GeometryArrowLayer_Value,
                    getUndo: (tLayer) => tLayer.Value,
                    setUndo: (tLayer, previous) => tLayer.Value = previous
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
                this.SelectionViewModel.GeometryArrow_Value = value2;

                this.MethodViewModel.TLayerChangeDelta<GeometryArrowLayer>(layerType: LayerType.GeometryArrow, set: (tLayer) => tLayer.Value = value2);
            };
            this.ValueSlider.ValueChangeCompleted += (sender, value) =>
            {
                float value2 = (float)value;
                this.SelectionViewModel.GeometryArrow_Value = value2;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.Value = value2,

                    type: HistoryType.LayersProperty_Set_GeometryArrowLayer_Value,
                    getUndo: (tLayer) => tLayer.StartingValue,
                    setUndo: (tLayer, previous) => tLayer.Value = previous
                );
            };
        }


        //LeftTail
        private void ConstructLeftTail()
        {
            this.LeftTailComboBox.TypeChanged += (s, type) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)type;
                this.SelectionViewModel.GeometryArrow_LeftTail = tailType;

                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.LeftTail = tailType,

                    type: HistoryType.LayersProperty_Set_GeometryArrowLayer_LeftTail,
                    getUndo: (tLayer) => tLayer.LeftTail,
                    setUndo: (tLayer, previous) => tLayer.LeftTail = previous
                );
            };
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightTailComboBox.TypeChanged += (s, type) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)type;
                this.SelectionViewModel.GeometryArrow_RightTail = tailType;

                this.MethodViewModel.TLayerChanged<GeometryArrowTailType, GeometryArrowLayer>
                (
                    layerType: LayerType.GeometryArrow,
                    set: (tLayer) => tLayer.RightTail = tailType,

                    type: HistoryType.LayersProperty_Set_GeometryArrowLayer_RightTail,
                    getUndo: (tLayer) => tLayer.RightTail,
                    setUndo: (tLayer, previous) => tLayer.RightTail = previous
                );
            };
        }

    }
}