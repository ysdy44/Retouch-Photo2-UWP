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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public partial class GeometryRoundRectTool : Page, ITool
    {  
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
       
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
            this.ConstructCorner();
        }


        //Corner
        private void ConstructCorner()
        {
            this.CornerTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.CornerTouchbarSlider.Unit = "%";
            this.CornerTouchbarSlider.NumberMinimum = 0;
            this.CornerTouchbarSlider.NumberMaximum = 50;
            this.CornerTouchbarSlider.NumberChange += (sender, number) =>
            {
                float corner = number / 100.0f;
                this.CornerChange(corner);
            };

            //Value
            this.CornerTouchbarSlider.Minimum = 0d;
            this.CornerTouchbarSlider.Maximum = 50d;
            this.CornerTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.CornerTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                this.CornerChange(corner);
            };
            this.CornerTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void CornerChange(float corner)
        {
            if (corner < 0.0f) corner = 0.0f;
            if (corner > 0.5f) corner = 0.5f;

            this.SelectionViewModel.GeometryRoundRectCorner = corner;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryRoundRectLayer roundRectLayer)
                {
                    roundRectLayer.Corner = corner;
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
    /// <see cref="ITool"/>'s GeometryRoundRectTool.
    /// </summary>
    public sealed partial class GeometryRoundRectTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryRoundRect");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CornerTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryRoundRect_Corner");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryRoundRect;
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
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManager()
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