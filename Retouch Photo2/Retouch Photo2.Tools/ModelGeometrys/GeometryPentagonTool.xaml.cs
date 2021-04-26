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
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>  
    public partial class GeometryPentagonTool : Page, ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;        
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;


        //@Content
        public ToolType Type => ToolType.GeometryPentagon;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GeometryPentagonTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "GeometryPentagonTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(GeometryPentagonTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a GeometryPentagonTool. 
        /// </summary>
        public GeometryPentagonTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.PointsButton.Tapped += (s, e) => TouchbarExtension.Instance = this.PointsButton;
            this.ConstructPoints1();
            this.ConstructPoints2();

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
            return new GeometryPentagonLayer
            {
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


    public partial class GeometryPentagonTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.PointsTextBlock.Text = resource.GetString("Tools_GeometryPentagon_Points");

            this.ConvertToCurvesToolTip.Content = resource.GetString("Tools_ConvertToCurves");

            this.MoreCreateToolTip.Content = resource.GetString("Tools_More");
        }


        //Points
        private void ConstructPoints1()
        {
            this.PointsPicker.Minimum = 3;
            this.PointsPicker.Maximum = 36;
            this.PointsPicker.ValueChanged += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChanged<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryPentagonLayer_Points,
                    getUndo: (tLayer) => tLayer.Points,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

        private void ConstructPoints2()
        {
            this.PointsSlider.Minimum = 3;
            this.PointsSlider.Maximum = 36;
            this.PointsSlider.ValueChangeStarted += (sender, value) => this.MethodViewModel.TLayerChangeStarted<GeometryPentagonLayer>(layerType: LayerType.GeometryPentagon, cache: (tLayer) => tLayer.CachePoints());
            this.PointsSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChangeDelta<GeometryPentagonLayer>(layerType: LayerType.GeometryPentagon, set: (tLayer) => tLayer.Points = points);
            };
            this.PointsSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                this.SelectionViewModel.GeometryPentagon_Points = points;

                this.MethodViewModel.TLayerChangeCompleted<int, GeometryPentagonLayer>
                (
                    layerType: LayerType.GeometryPentagon,
                    set: (tLayer) => tLayer.Points = points,

                    type: HistoryType.LayersProperty_Set_GeometryPentagonLayer_Points,
                    getUndo: (tLayer) => tLayer.StartingPoints,
                    setUndo: (tLayer, previous) => tLayer.Points = previous
                );
            };
        }

    }
}