// Core:              ★★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
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
    public partial class GeometryArrowTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private string ValueToStringConverter(float sweepAngle) => $"{this.ValueToNumberConverter(sweepAngle)}%";
        private int ValueToNumberConverter(float value) => (int)(value * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryArrow;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryArrowTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryArrowTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryArrowTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryArrowTool. 
        /// </summary>
        public GeometryArrowTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ValueButton.Tapped += (s, e) => TouchbarExtension.Instance = this.ValueButton;
            this.ConstructValue1();
            this.ConstructValue2();

            this.ConstructLeftTail();
            this.ConstructRightTail();

            this.ConvertToCurvesButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.ViewModel.ToolType = ToolType.Node;
            };

            this.MoreButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryArrowLayer
            {
                LeftTail = this.SelectionViewModel.GeometryArrow_LeftTail,
                RightTail = this.SelectionViewModel.GeometryArrow_RightTail,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.ClickeTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }
    }


    public partial class GeometryArrowTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ValueTextBlock.Text = resource.GetString("Tools_GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("Tools_GeometryArrow_LeftTail");
            this.RightTailTextBlock.Text = resource.GetString("Tools_GeometryArrow_RightTail");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }


        //Value
        private void ConstructValue1()
        {
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