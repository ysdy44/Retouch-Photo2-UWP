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
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public partial class GeometryHeartTool : Page, ITool
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;
      
        private int SpreadToNumberConverter(float spread) => (int)(spread * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryHeart;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryHeartTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryHeartTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryHeartTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryHeartTool. 
        /// </summary>
        public GeometryHeartTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSpread1();
            this.ConstructSpread2();

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
            return new GeometryHeartLayer
            {
                Spread = this.SelectionViewModel.GeometryHeart_Spread,
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


    public partial class GeometryHeartTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.SpreadTextBlock.Text = resource.GetString("Tools_GeometryHeart_Spread");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_MoreCreate");
        }


        //Spead
        private void ConstructSpread1()
        {
            this.SpreadPicker.Unit = "%";
            this.SpreadPicker.Minimum = 0;
            this.SpreadPicker.Maximum = 100;
            this.SpreadPicker.ValueChanged += (sender, value) =>
            {
                float spread = (float)value / 100.0f;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChanged<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    set: (tLayer) => tLayer.Spread = spread,

                    type: HistoryType.LayersProperty_Set_GeometryHeartLayer_Spread,
                    getUndo: (tLayer) => tLayer.Spread,
                    setUndo: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

        private void ConstructSpread2()
        {
            this.SpreadSlider.Minimum = 0.0d;
            this.SpreadSlider.Maximum = 1.0d;
            this.SpreadSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryHeartLayer>(layerType: LayerType.GeometryHeart, cache: (tLayer) => tLayer.CacheSpread());
            this.SpreadSlider.ValueChangeDelta += (sender, value) =>
            {
                float spread = (float)value;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChangeDelta<GeometryHeartLayer>(layerType: LayerType.GeometryHeart, set: (tLayer) => tLayer.Spread = spread);
            };
            this.SpreadSlider.ValueChangeCompleted += (sender, value) =>
            {
                float spread = (float)value;
                this.SelectionViewModel.GeometryHeart_Spread = spread;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryHeartLayer>
                (
                    layerType: LayerType.GeometryHeart,
                    set: (tLayer) => tLayer.Spread = spread,

                    type: HistoryType.LayersProperty_Set_GeometryHeartLayer_Spread,
                    getUndo: (tLayer) => tLayer.StartingSpread,
                    setUndo: (tLayer, previous) => tLayer.Spread = previous
                );
            };
        }

    }
}