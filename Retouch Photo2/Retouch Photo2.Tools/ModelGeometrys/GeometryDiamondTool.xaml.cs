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
    /// <summary>
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public partial class GeometryDiamondTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;        
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private int MidToNumberConverter(float mid) => (int)(mid * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryDiamond;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryDiamondTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryDiamondTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryDiamondTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryDiamondTool. 
        /// </summary>
        public GeometryDiamondTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructMid1();
            this.ConstructMid2();
            this.ConstructMirror();

            this.ConvertToCurvesButton.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.ViewModel.ToolType = ToolType.Node;
            };

            this.MoreButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }

        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryDiamondLayer
            {
                Mid = this.SelectionViewModel.GeometryDiamond_Mid,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.ClickeTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }
    }


    public partial class GeometryDiamondTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.MidTextBlock.Text = resource.GetString("Tools_GeometryDiamond_Mirror");
            this.MirrorTextBlock.Text = resource.GetString("Tools_GeometryDiamond_Mid");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_More");
        }


        //Mid
        private void ConstructMid1()
        {
            this.MidPicker.Unit = "%";
            this.MidPicker.Minimum = 0;
            this.MidPicker.Maximum = 100;
            this.MidPicker.ValueChanged += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.Mid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMid2()
        {
            this.MidSlider.Minimum = 0.0d;
            this.MidSlider.Maximum = 1.0d;
            this.MidSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, cache: (tLayer) => tLayer.CacheMid());
            this.MidSlider.ValueChangeDelta += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChangeDelta<GeometryDiamondLayer>(layerType: LayerType.GeometryDiamond, set: (tLayer) => tLayer.Mid = mid);
            };
            this.MidSlider.ValueChangeCompleted += (sender, value) =>
            {
                float mid = (float)value;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.StartingMid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                float mid = 1.0f - this.SelectionViewModel.GeometryDiamond_Mid;
                this.SelectionViewModel.GeometryDiamond_Mid = mid;

                this.MethodViewModel.TLayerChanged<float, GeometryDiamondLayer>
                (
                    layerType: LayerType.GeometryDiamond,
                    set: (tLayer) => tLayer.Mid = 1.0f - tLayer.Mid,

                    type: HistoryType.LayersProperty_Set_GeometryDiamondLayer_Mid,
                    getUndo: (tLayer) => tLayer.Mid,
                    setUndo: (tLayer, previous) => tLayer.Mid = previous
                );
            };
        }

    }
}