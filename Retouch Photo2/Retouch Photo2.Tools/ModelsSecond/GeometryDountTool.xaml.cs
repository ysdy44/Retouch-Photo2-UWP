using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
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
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public sealed partial class GeometryDountTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        internal bool TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case false:
                        this.HoleRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.HoleRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.HoleRadiusTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int HoleRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double HoleRadiusValueConverter(float innerRadius) => innerRadius * 100d;


        //@Construct
        public GeometryDountTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructHoleRadius1();
            this.ConstructHoleRadius2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryDount");
            this._button.Style = this.IconSelectedButtonStyle;

            this.HoleRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDount_HoleRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDount;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryDountIcon();
        readonly Button _button = new Button { Tag = new GeometryDountIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryDountLayer
            {
                HoleRadius = this.SelectionViewModel.GeometryDountHoleRadius,
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
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public partial class GeometryDountTool : Page, ITool
    {

        //HoleRadius
        private void ConstructHoleRadius1()
        {
            //Button
            this.HoleRadiusTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.HoleRadiusTouchbarSlider.Unit = "%";
            this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
            this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
            this.HoleRadiusTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set dount layer HoleRadius");

                //Selection
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryDount)
                    {
                        GeometryDountLayer geometryDountLayer = (GeometryDountLayer)layer;

                        var previous = geometryDountLayer.HoleRadius;
                        history.UndoActions.Push(() =>
                        {
                            GeometryDountLayer layer2 = geometryDountLayer;

                            layer2.HoleRadius = previous;
                        });

                        geometryDountLayer.HoleRadius = holeRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructHoleRadius2()
        {
            //History
            LayersPropertyHistory history = null;

            //Value
            this.HoleRadiusTouchbarSlider.Unit = "%";
            this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
            this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
            this.HoleRadiusTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                history = new LayersPropertyHistory("Set dount layer hole radius");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryDount)
                    {
                        GeometryDountLayer geometryDountLayer = (GeometryDountLayer)layer;
                        geometryDountLayer.CacheHoleRadius();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.HoleRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryDount)
                    {
                        GeometryDountLayer geometryDountLayer = (GeometryDountLayer)layer;
                        geometryDountLayer.HoleRadius = holeRadius;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.HoleRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float holeRadius = (float)value / 100.0f;
                if (holeRadius < 0.0f) holeRadius = 0.0f;
                if (holeRadius > 1.0f) holeRadius = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryDountHoleRadius = holeRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryDount)
                    {
                        GeometryDountLayer geometryDountLayer = (GeometryDountLayer)layer;

                        var previous = geometryDountLayer.StartingHoleRadius;
                        history.UndoActions.Push(() =>
                        {
                            GeometryDountLayer layer2 = geometryDountLayer;

                            layer2.HoleRadius = previous;
                        });

                        geometryDountLayer.HoleRadius = holeRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}