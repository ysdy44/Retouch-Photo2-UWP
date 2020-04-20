using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public sealed partial class GeometryHeartTool : Page, ITool
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
                    this.SpreadTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.SpreadTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.SpreadTouchbarSlider;
                }
            }
        }


        //@Converter
        private int SpreadNumberConverter(float spread) => (int)(spread * 100.0f);
        private double SpreadValueConverter(float spread) => spread * 100d;


        //@Construct
        public GeometryHeartTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructSpread();

            this.CreateTool = new CreateTool
            {
                CreateLayer = (Transformer transformer) =>
                {
                    return new GeometryHeartLayer
                    {
                        Spread = this.SelectionViewModel.GeometryHeartSpread,
                    };
                }
            };
        }


        //Spead
        private void ConstructSpread()
        {
            //Button
            this.SpreadTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.SpreadTouchbarSlider.Unit = "%";
            this.SpreadTouchbarSlider.NumberMinimum = 0;
            this.SpreadTouchbarSlider.NumberMaximum = 100;
            this.SpreadTouchbarSlider.NumberChange += (sender, number) =>
            {
                float spread = number / 100.0f;
                this.SpreadChange(spread);
            };

            //Value
            this.SpreadTouchbarSlider.Minimum = 0d;
            this.SpreadTouchbarSlider.Maximum = 100d;
            this.SpreadTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.SpreadTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float spread = (float)value / 100.0f;
                this.SpreadChange(spread);
            };
            this.SpreadTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void SpreadChange(float spread)
        {
            if (spread < 0.0f) spread = 0.0f;
            if (spread > 1.0f) spread = 1.0f;

            this.SelectionViewModel.GeometryHeartSpread = spread;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryHeartLayer geometryHeartLayer)
                {
                    geometryHeartLayer.Spread = spread;
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
    /// <see cref="ITool"/>'s GeometryHeartTool.
    /// </summary>
    public partial class GeometryHeartTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/ToolsSecond/GeometryHeart");

            this.SpreadTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryHeart_Spread");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryHeart;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryHeartIcon();
        readonly ToolSecondButton _button = new ToolSecondButton(new GeometryHeartIcon());

        readonly CreateTool CreateTool;


        public void Starting(Vector2 point) => this.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.CreateTool.Started(startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.CreateTool.Draw(drawingSession);

    }
}