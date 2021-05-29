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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;

        private string CenterToStringConverter(float center) => $"{this.CenterToNumberConverter(center)}%";
        private int CenterToNumberConverter(float center) => (int)(center * 100.0f);


        //@Content
        public ToolType Type => ToolType.GeometryTriangle;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryTriangleTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryTriangleTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryTriangleTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryTriangleTool. 
        /// </summary>
        public GeometryTriangleTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.CenterButton.Tapped += (s, e) => TouchbarExtension.Instance = this.CenterButton;
            this.ConstructCenter1();
            this.ConstructCenter2();

            this.ConstructMirror();

            this.ConvertToCurvesButton.Tapped += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.None) return;

                this.MethodViewModel.MethodConvertToCurves();

                // Change tools group value.
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
            return new GeometryTriangleLayer
            {
                Center = this.SelectionViewModel.GeometryTriangle_Center,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandardGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.ViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.ViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.CreateTool.Cursor(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.ViewModel.CreateTool.Draw(drawingSession);


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate(); // Invalidate
        }
        public void OnNavigatedFrom()
        {
            TouchbarExtension.Instance = null;
        }
    }


    public partial class GeometryTriangleTool : Page, ITool
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.CenterTextBlock.Text = resource.GetString("Tools_GeometryTriangle_Center");
            this.MirrorTextBlock.Text = resource.GetString("Tools_GeometryTriangle_Mirror");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }

        // Center
        private void ConstructCenter1()
        {
            this.CenterPicker.Minimum = 0;
            this.CenterPicker.Maximum = 100;
            this.CenterPicker.ValueChanged += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    layerType: LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.Center,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructCenter2()
        {
            this.CenterSlider.Minimum = 0.0d;
            this.CenterSlider.Maximum = 1.0d;
            this.CenterSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TLayerChangeStarted<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, cache: (tLayer) => tLayer.CacheCenter());
            this.CenterSlider.ValueChangeDelta += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChangeDelta<GeometryTriangleLayer>(layerType: LayerType.GeometryTriangle, set: (tLayer) => tLayer.Center = center);
            };
            this.CenterSlider.ValueChangeCompleted += (s, value) =>
            {
                float center = (float)value;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChangeCompleted<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.StartingCenter,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Tapped += (s, e) =>
            {
                float center = 1.0f - this.SelectionViewModel.GeometryTriangle_Center;
                this.SelectionViewModel.GeometryTriangle_Center = center;

                this.MethodViewModel.TLayerChanged<float, GeometryTriangleLayer>
                (
                    LayerType.GeometryTriangle,
                    set: (tLayer) => tLayer.Center = 1.0f - tLayer.Center,

                    type: HistoryType.LayersProperty_Set_GeometryTriangleLayer_Center,
                    getUndo: (tLayer) => tLayer.Center,
                    setUndo: (tLayer, previous) => tLayer.Center = previous
                );
            };
        }

    }
}