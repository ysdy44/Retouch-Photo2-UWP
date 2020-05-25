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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : Page, ITool
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
                if (value == false)
                {
                    this.CenterTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.CenterTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.CenterTouchbarSlider;
                }
            }
        }

        //@Converter
        private int CenterNumberConverter(float center) => (int)(center * 100.0f);
        private double CenterValueConverter(float center) => center * 100d;


        //@Construct
        public GeometryTriangleTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructCenter1();
            this.ConstructCenter2();
            this.ConstructMirror();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public sealed partial class GeometryTriangleTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryTriangle");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CenterTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryTriangle_Center");
            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryTriangle_Mirror");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryTriangle;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryTriangleIcon();
        readonly Button _button = new Button { Tag = new GeometryTriangleIcon() };

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryTriangleLayer
            {
                Center = this.SelectionViewModel.GeometryTriangleCenter,
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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public sealed partial class GeometryTriangleTool : Page, ITool
    {

        private void ConstructCenter1()
        {
            //Button
            this.CenterTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.CenterTouchbarSlider.Unit = "%";
            this.CenterTouchbarSlider.NumberMinimum = 0;
            this.CenterTouchbarSlider.NumberMaximum = 100;
            this.CenterTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                if (center < 0.0f) center = 0.0f;
                if (center > 1.0f) center = 1.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set triangle layer center");

                //Selection
                this.SelectionViewModel.GeometryTriangleCenter = center;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryTriangle)
                    {
                        GeometryTriangleLayer geometryTriangleLayer = (GeometryTriangleLayer)layer;

                        var previous = geometryTriangleLayer.Center;
                        history.UndoActions.Push(() =>
                        {
                            GeometryTriangleLayer layer2 = geometryTriangleLayer;

                            layer2.Center = previous;
                        });

                        geometryTriangleLayer.Center = center;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructCenter2()
        {
            //History
            LayersPropertyHistory history = null;

            //Value
            this.CenterTouchbarSlider.Minimum = 0d;
            this.CenterTouchbarSlider.Maximum = 100d;
            this.CenterTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                history = new LayersPropertyHistory("Set triangle layer center");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryTriangle)
                    {
                        GeometryTriangleLayer geometryTriangleLayer = (GeometryTriangleLayer)layer;
                        geometryTriangleLayer.CacheCenter();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.CenterTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                if (center < 0.0f) center = 0.0f;
                if (center > 1.0f) center = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryTriangleCenter = center;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryTriangle)
                    {
                        GeometryTriangleLayer geometryTriangleLayer = (GeometryTriangleLayer)layer;
                        geometryTriangleLayer.Center = center;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.CenterTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                if (center < 0.0f) center = 0.0f;
                if (center > 1.0f) center = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryTriangleCenter = center;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryTriangle)
                    {
                        GeometryTriangleLayer geometryTriangleLayer = (GeometryTriangleLayer)layer;

                        var previous = geometryTriangleLayer.StartingCenter;
                        history.UndoActions.Push(() =>
                        {
                            GeometryTriangleLayer layer2 = geometryTriangleLayer;

                            layer2.Center = previous;
                        });

                        geometryTriangleLayer.Center = center;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

        private void ConstructMirror()
        {
            this.MirrorButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set triangle layer center");

                //Selection
                float selectionCenter = 1.0f - this.SelectionViewModel.GeometryTriangleCenter;
                this.SelectionViewModel.GeometryTriangleCenter = selectionCenter;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryTriangle)
                    {
                        GeometryTriangleLayer geometryTriangleLayer = (GeometryTriangleLayer)layer;

                        var previous = geometryTriangleLayer.Center;
                        history.UndoActions.Push(() =>
                        {
                            GeometryTriangleLayer layer2 = geometryTriangleLayer;

                            layer2.Center = previous;
                        });

                        float center = 1.0f - geometryTriangleLayer.Center;
                        geometryTriangleLayer.Center = center;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}