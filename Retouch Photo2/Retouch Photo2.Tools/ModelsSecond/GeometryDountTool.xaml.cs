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
    /// <see cref="ITool"/>'s GeometryDountTool.
    /// </summary>
    public sealed partial class GeometryDountTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

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
            this.ConstructHoleRadius();
        }


        //HoleRadius
        private void ConstructHoleRadius()
        {
            //Button
            this.HoleRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = true;
                else
                    this.TouchBarMode = false;
            };

            //Number
            this.HoleRadiusTouchbarSlider.Unit = "%";
            this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
            this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
            this.HoleRadiusTouchbarSlider.NumberChange += (sender, number) =>
            {
                float innerRadius = number / 100f;
                this.HoleRadiusChange(innerRadius);
            };

            //Value
            this.HoleRadiusTouchbarSlider.Minimum = 0d;
            this.HoleRadiusTouchbarSlider.Maximum = 100d;
            this.HoleRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.HoleRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)(value / 100d);
                this.HoleRadiusChange(innerRadius);
            };
            this.HoleRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void HoleRadiusChange(float innerRadius)
        {
            this.SelectionViewModel.GeometryDountHoleRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryDount)
                {
                    GeometryDountLayer geometryDountLayer = (GeometryDountLayer)layer;
                    geometryDountLayer.HoleRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
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

            this._button.Content = resource.GetString("/ToolsSecond/GeometryDount");
            this._button.Style = this.IconSelectedButtonStyle;

            this.HoleRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDount_HoleRadius");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDount;
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
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManagerGeometry()
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance); public void Clicke(Vector2 point) => this.TipViewModel.TransformerTool.SelectSingleLayer(point);//Select single layer

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}