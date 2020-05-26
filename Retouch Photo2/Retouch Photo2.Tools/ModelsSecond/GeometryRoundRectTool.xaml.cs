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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public partial class GeometryRoundRectTool : Page, ITool
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
                    this.CornerTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.CornerTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.CornerTouchbarSlider;
                }
            }
        }
        
        //@Converter
        private int CornerNumberConverter(float corner) => (int)(corner * 100.0f);
        private double CornerValueConverter(float corner) => corner * 100d;


        //@Construct
        public GeometryRoundRectTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructCorner1();
            this.ConstructCorner2();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryRoundRect");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CornerTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryRoundRect_Corner");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryRoundRectIcon();
        readonly Button _button = new Button { Tag = new GeometryRoundRectIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryRoundRectLayer
            {
                Corner = this.SelectionViewModel.GeometryRoundRectCorner,
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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {

        private void ConstructCorner1()
        {
            //Button
            this.CornerTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.CornerTouchbarSlider.Unit = "%";
            this.CornerTouchbarSlider.NumberMinimum = 0;
            this.CornerTouchbarSlider.NumberMaximum = 50;
            this.CornerTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                if (corner < 0.0f) corner = 0.0f;
                if (corner > 0.5f) corner = 0.5f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set round rect corner");

                //Selection
                this.SelectionViewModel.GeometryRoundRectCorner = corner;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryRoundRect)
                    {
                        GeometryRoundRectLayer geometryRoundRectLayer = (GeometryRoundRectLayer)layer;

                        var previous = geometryRoundRectLayer.Corner;
                        history.UndoActions.Push(() =>
                        {
                            GeometryRoundRectLayer layer2 = geometryRoundRectLayer;

                            layer2.Corner = previous;
                        });

                        geometryRoundRectLayer.Corner = corner;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructCorner2()
        {
            //History
            LayersPropertyHistory history = null;

            //Value
            this.CornerTouchbarSlider.Minimum = 0d;
            this.CornerTouchbarSlider.Maximum = 50d;
            this.CornerTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                history = new LayersPropertyHistory("Set round rect corner");

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryRoundRect)
                    {
                        GeometryRoundRectLayer geometryRoundRectLayer = (GeometryRoundRectLayer)layer;
                        geometryRoundRectLayer.CacheCorner();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.CornerTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                if (corner < 0.0f) corner = 0.0f;
                if (corner > 0.5f) corner = 0.5f;

                //Selection
                this.SelectionViewModel.GeometryRoundRectCorner = corner;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryRoundRect)
                    {
                        GeometryRoundRectLayer geometryRoundRectLayer = (GeometryRoundRectLayer)layer;
                        geometryRoundRectLayer.Corner = corner;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.CornerTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                if (corner < 0.0f) corner = 0.0f;
                if (corner > 0.5f) corner = 0.5f;

                //Selection
                this.SelectionViewModel.GeometryRoundRectCorner = corner;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryRoundRect)
                    {
                        GeometryRoundRectLayer geometryRoundRectLayer = (GeometryRoundRectLayer)layer;

                        var previous = geometryRoundRectLayer.StartingCorner;
                        history.UndoActions.Push(() =>
                        {
                            GeometryRoundRectLayer layer2 = geometryRoundRectLayer;

                            layer2.Corner = previous;
                        });

                        geometryRoundRectLayer.Corner = corner;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}