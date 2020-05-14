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
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public partial class GeometryDiamondTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        internal bool TouchBarMode
        {
            set
            {
                if (value == false)
                {
                    this.MidTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.MidTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.MidTouchbarSlider;
                }
            }
        }


        //@Converter
        private int MidNumberConverter(float mid) => (int)(mid * 100.0f);
        private double MidValueConverter(float mid) => mid * 100d;


        //@Construct
        public GeometryDiamondTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMid();

            this.MirrorButton.Tapped += (s, e) => this.MidMirror();
        }


        //Mid
        private void ConstructMid()
        {
            //Button
            this.MidTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.MidTouchbarSlider.Unit = "%";
            this.MidTouchbarSlider.NumberMinimum = 0;
            this.MidTouchbarSlider.NumberMaximum = 100;
            this.MidTouchbarSlider.NumberChange += (sender, number) =>
            {
                float mid = number / 100.0f;
                this.MidChange(mid);
            };

            //Value
            this.MidTouchbarSlider.Minimum = 0d;
            this.MidTouchbarSlider.Maximum = 100d;
            this.MidTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.MidTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float mid = (float)value / 100.0f;
                this.MidChange(mid);
            };
            this.MidTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void MidChange(float mid)
        {
            if (mid < 0.0f) mid = 0.0f;
            if (mid > 1.0f) mid = 1.0f;

            this.SelectionViewModel.GeometryDiamondMid = mid;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryDiamond)
                {
                    GeometryDiamondLayer geometryDiamondLayer = (GeometryDiamondLayer)layer;
                    geometryDiamondLayer.Mid = mid;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        private void MidMirror()
        {
            float selectionMid = 1.0f - this.SelectionViewModel.GeometryDiamondMid;
            this.SelectionViewModel.GeometryDiamondMid = selectionMid;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryDiamond)
                {
                    GeometryDiamondLayer geometryDiamondLayer = (GeometryDiamondLayer)layer;
                    float mid = 1.0f - geometryDiamondLayer.Mid;
                    geometryDiamondLayer.Mid = mid;
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
    /// <see cref="ITool"/>'s GeometryDiamondTool.
    /// </summary>
    public sealed partial class GeometryDiamondTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryDiamond");
            this._button.Style = this.IconSelectedButtonStyle;

            this.MirrorTextBlock.Text = resource.GetString("/ToolsSecond/GeometryDiamond_Mirror");
            this.MidTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryDiamond_Mid");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryDiamond;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryDiamondIcon();
        readonly Button _button = new Button { Tag = new GeometryDiamondIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryDiamondLayer
            {
                Mid = this.SelectionViewModel.GeometryDiamondMid,
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManagerGeometry()
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.TransformerTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}