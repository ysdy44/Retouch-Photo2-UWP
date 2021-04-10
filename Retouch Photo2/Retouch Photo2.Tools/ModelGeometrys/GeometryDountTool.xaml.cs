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
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private int HoleRadiusToNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryDount;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryDountTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryDountTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryDountTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryDountPage. 
        /// </summary>
        public GeometryDountTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructHoleRadius1();
            this.ConstructHoleRadius2();

            this.ConvertToCurvesButton.Click += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                //Change tools group value.
                this.TipViewModel.ToolType = ToolType.Node;
            };

            this.MoreCreateButton.Click += (s, e) => Retouch_Photo2.DrawPage.ShowMoreCreate?.Invoke(this, this.MoreCreateButton);
        }


        /// <summary>
        /// Create a ILayer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The producted ILayer. </returns>
        public ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryDountLayer
            {
                HoleRadius = this.SelectionViewModel.GeometryDount_HoleRadius,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }

        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.TipViewModel.ClickeTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }
    }


    public partial class GeometryDountTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.HoleRadiusTextBlock.Text = resource.GetString("Tools_GeometryDount_HoleRadius");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }


        //HoleRadius
        private void ConstructHoleRadius1()
        {
            this.HoleRadiusPicker.Unit = "%";
            this.HoleRadiusPicker.Minimum = 0;
            this.HoleRadiusPicker.Maximum = 100;
            this.HoleRadiusPicker.ValueChanged += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                this.SelectionViewModel.GeometryDount_HoleRadius = holeRadius;

                this.MethodViewModel.TLayerChanged<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    type: HistoryType.LayersProperty_Set_GeometryDountLayer_HoleRadius,
                    getUndo: (tLayer) => tLayer.HoleRadius,
                    setUndo: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }

        private void ConstructHoleRadius2()
        {
            this.HoleRadiusSlider.Minimum = 0.0d;
            this.HoleRadiusSlider.Maximum = 1.0d;
            this.HoleRadiusSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryDountLayer>(layerType: LayerType.GeometryDount, cache: (tLayer) => tLayer.CacheHoleRadius());
            this.HoleRadiusSlider.ValueChangeDelta += (sender, value) =>
            {
                float holeRadius = (float)value;
                this.SelectionViewModel.GeometryDount_HoleRadius = holeRadius;

                this.MethodViewModel.TLayerChangeDelta<GeometryDountLayer>(layerType: LayerType.GeometryDount, set: (tLayer) => tLayer.HoleRadius = holeRadius);
            };
            this.HoleRadiusSlider.ValueChangeCompleted += (sender, value) =>
            {
                float holeRadius = (float)value;
                this.SelectionViewModel.GeometryDount_HoleRadius = holeRadius;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryDountLayer>
                (
                    layerType: LayerType.GeometryDount,
                    set: (tLayer) => tLayer.HoleRadius = holeRadius,

                    type: HistoryType.LayersProperty_Set_GeometryDountLayer_HoleRadius,
                    getUndo: (tLayer) => tLayer.StartingHoleRadius,
                    setUndo: (tLayer, previous) => tLayer.HoleRadius = previous
                );
            };
        }

    }
}