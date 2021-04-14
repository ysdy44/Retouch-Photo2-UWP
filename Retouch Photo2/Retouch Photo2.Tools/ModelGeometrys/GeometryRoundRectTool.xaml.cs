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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public partial class GeometryRoundRectTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;        
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private int CornerToNumberConverter(float corner) => (int)(corner * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryRoundRectTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryRoundRectTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryRoundRectTool), new PropertyMetadata(false));


        #endregion


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
            return new GeometryRoundRectLayer
            {
                Corner = this.SelectionViewModel.GeometryRoundRect_Corner,
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


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarButton.Instance = null;
        }
    }


    public partial class GeometryRoundRectTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CornerTextBlock.Text = resource.GetString("Tools_GeometryRoundRect_Corner");
            
            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_More");
        }

        //Corner
        private void ConstructCorner1()
        {
            this.CornerPicker.Unit = "%";
            this.CornerPicker.Minimum = 0;
            this.CornerPicker.Maximum = 50;
            this.CornerPicker.ValueChanged += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChanged<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    type: HistoryType.LayersProperty_Set_GeometryRoundRectLayer_Corner,
                    getUndo: (tLayer) => tLayer.Corner,
                    setUndo: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

        private void ConstructCorner2()
        {
            this.CornerSlider.Minimum = 0.0d;
            this.CornerSlider.Maximum = 0.5d;
            this.CornerSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryRoundRectLayer>(layerType: LayerType.GeometryRoundRect, cache: (tLayer) => tLayer.CacheCorner());
            this.CornerSlider.ValueChangeDelta += (sender, value) =>
            {
                float corner = (float)value;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChangeDelta<GeometryRoundRectLayer>(layerType: LayerType.GeometryRoundRect, set: (tLayer) => tLayer.Corner = corner);
            };
            this.CornerSlider.ValueChangeCompleted += (sender, value) =>
            {
                float corner = (float)value;
                this.SelectionViewModel.GeometryRoundRect_Corner = corner;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryRoundRectLayer>
                (
                    layerType: LayerType.GeometryRoundRect,
                    set: (tLayer) => tLayer.Corner = corner,

                    type: HistoryType.LayersProperty_Set_GeometryRoundRectLayer_Corner,
                    getUndo: (tLayer) => tLayer.StartingCorner,
                    setUndo: (tLayer, previous) => tLayer.Corner = previous
                );
            };
        }

    }
}