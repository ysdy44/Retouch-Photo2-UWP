using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

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

            this.ConstructPoints1();
            this.ConstructPoints2();
            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();
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

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryStar");
            this._button.Style = this.IconSelectedButtonStyle;

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_Points");
            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryStar_InnerRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryStar;
        public string Title { get; set; }
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
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.GeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryStarTool.
    /// </summary>
    public sealed partial class GeometryStarTool : Page, ITool
    {

        //Points
        private void ConstructPoints1()
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
            this.PointsTouchbarSlider.ValueChanged += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set star layer points");

                //Selection
                this.SelectionViewModel.GeometryStarPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;

                        var previous = geometryStarLayer.Points;
                        history.UndoActions.Push(() =>
                        {
                            GeometryStarLayer layer2 = geometryStarLayer;

                            layer2.Points = previous;
                        });

                        geometryStarLayer.Points = points;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructPoints2()
        {
            //History
            LayersPropertyHistory history = null;

            //Value
            this.PointsTouchbarSlider.Minimum = 3d;
            this.PointsTouchbarSlider.Maximum = 36d;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                history = new LayersPropertyHistory("Set star layer points");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                        geometryStarLayer.CachePoints();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //Selection
                this.SelectionViewModel.GeometryStarPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                        geometryStarLayer.Points = points;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //Selection
                this.SelectionViewModel.GeometryStarPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;

                        var previous = geometryStarLayer.StartingPoints;
                        history.UndoActions.Push(() =>
                        {
                            GeometryStarLayer layer2 = geometryStarLayer;

                            layer2.Points = previous;
                        });

                        geometryStarLayer.Points = points;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

        //InnerRadius
        private void ConstructInnerRadius1()
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
            this.InnerRadiusTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                if (innerRadius < 0.0f) innerRadius = 0.0f;
                if (innerRadius > 1.0f) innerRadius = 1.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set star layer inner radius");

                //Selection
                this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;

                        var previous = geometryStarLayer.InnerRadius;
                        history.UndoActions.Push(() =>
                        {
                            GeometryStarLayer layer2 = geometryStarLayer;

                            layer2.InnerRadius = previous;
                        });

                        geometryStarLayer.InnerRadius = innerRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructInnerRadius2()
        {
            //History
            LayersPropertyHistory history = null;

            //Value
            this.InnerRadiusTouchbarSlider.Minimum = 0d;
            this.InnerRadiusTouchbarSlider.Maximum = 100d;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                history = new LayersPropertyHistory("Set star layer innerRadius");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                        geometryStarLayer.CacheInnerRadius();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                if (innerRadius < 0.0f) innerRadius = 0.0f;
                if (innerRadius > 1.0f) innerRadius = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;
                        geometryStarLayer.InnerRadius = innerRadius;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value / 100.0f;
                if (innerRadius < 0.0f) innerRadius = 0.0f;
                if (innerRadius > 1.0f) innerRadius = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryStar)
                    {
                        GeometryStarLayer geometryStarLayer = (GeometryStarLayer)layer;

                        var previous = geometryStarLayer.StartingInnerRadius;
                        history.UndoActions.Push(() =>
                        {
                            GeometryStarLayer layer2 = geometryStarLayer;

                            layer2.InnerRadius = previous;
                        });

                        geometryStarLayer.InnerRadius = innerRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}