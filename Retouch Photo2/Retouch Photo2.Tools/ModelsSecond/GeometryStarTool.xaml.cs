using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
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
    /// <summary>
    /// Enum of <see cref="GeometryStarTool"/>.
    /// </summary>
    internal enum GeometryStarMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Points. </summary>
        Points,

        /// <summary> Inner-radius. </summary>
        InnerRadius,
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public partial class GeometryStarTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@TouchBar  
        internal GeometryStarMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryStarMode.None:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryStarMode.Points:
                        this.PointsTouchbarButton.IsSelected = true;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.PointsTouchbarSlider;
                        break;
                    case GeometryStarMode.InnerRadius:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.InnerRadiusTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private double PointsValueConverter(float points) => points;

        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;


        //@Construct
        public GeometryStarTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructPoints();
            this.ConstructInnerRadius();
        }


        //Points
        private void ConstructPoints()
        {
            //Button
            this.PointsTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryStarMode.Points;
                else
                    this.TouchBarMode = GeometryStarMode.None;
            };

            //Number
            this.PointsTouchbarSlider.NumberMinimum = 3;
            this.PointsTouchbarSlider.NumberMaximum = 36;
            this.PointsTouchbarSlider.NumberChange += (sender, number) =>
            {
                int points = number;
                this.PointsChange(points);
            };

            //Value
            this.PointsTouchbarSlider.Minimum = 3d;
            this.PointsTouchbarSlider.Maximum = 36d;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                this.PointsChange(points);
            };
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void PointsChange(int points)
        {
            if (points < 3) points = 3;
            if (points > 36) points = 36;

            this.SelectionViewModel.GeometryStarPoints = points;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryStar)
                {
                    GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                    geometryStarLayer.Points = points;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }


        //InnerRadius
        private void ConstructInnerRadius()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryStarMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryStarMode.None;
            };

            //Number
            this.InnerRadiusTouchbarSlider.Unit = "%";
            this.InnerRadiusTouchbarSlider.NumberMinimum = 0;
            this.InnerRadiusTouchbarSlider.NumberMaximum = 100;
            this.InnerRadiusTouchbarSlider.NumberChange += (sender, number) =>
            {
                float innerRadius = number / 100f;
                this.InnerRadiusChange(innerRadius);
            };

            //Value
            this.InnerRadiusTouchbarSlider.Minimum = 0d;
            this.InnerRadiusTouchbarSlider.Maximum = 100d;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)(value / 100d);
                this.InnerRadiusChange(innerRadius);
            };
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void InnerRadiusChange(float innerRadius)
        {
            this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryStar)
                {
                    GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                    geometryStarLayer.InnerRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryStarMode.None;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public sealed partial class GeometryStarTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryStar");
            this._button.Style = this.IconSelectedButtonStyle;

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_Points");
            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_InnerRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryStar;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryStarIcon();
        readonly Button _button = new Button { Tag = new GeometryStarIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryStarLayer
            {
                Points = this.SelectionViewModel.GeometryStarPoints,
                InnerRadius = this.SelectionViewModel.GeometryStarInnerRadius,
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManagerGeometry()
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.TransformerTool.SelectSingleLayer(point);//Select single layer

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}