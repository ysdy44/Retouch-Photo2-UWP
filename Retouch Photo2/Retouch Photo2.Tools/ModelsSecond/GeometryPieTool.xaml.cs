using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public partial class GeometryPieTool : Page, ITool
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
                switch (value)
                {
                    case false:
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.SweepAngleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.SweepAngleTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);
        private double SweepAngleValueConverter(float sweepAngle) => sweepAngle / System.Math.PI * 180d;


        //@Construct
        public GeometryPieTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructSweepAngle();
        }


        //SweepAngle
        private void ConstructSweepAngle()
        {
            //Button
            this.SweepAngleTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = true;
                else
                    this.TouchBarMode = false;
            };

            //Number
            this.SweepAngleTouchbarSlider.Unit = "º";
            this.SweepAngleTouchbarSlider.NumberMinimum = 0;
            this.SweepAngleTouchbarSlider.NumberMaximum = 360;
            this.SweepAngleTouchbarSlider.NumberChange += (sender, number) =>
            {
                float sweepAngle = number / 180f * FanKit.Math.Pi;
                this.SweepAngleChange(sweepAngle);
            };

            //Value
            this.SweepAngleTouchbarSlider.Minimum = 0d;
            this.SweepAngleTouchbarSlider.Maximum = 360d;
            this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                this.SweepAngleChange(sweepAngle);
            };
            this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void SweepAngleChange(float sweepAngle)
        {
            this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryPieLayer geometryPieLayer)
                {
                    geometryPieLayer.SweepAngle = sweepAngle;
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
    /// <see cref="ITool"/>'s GeometryPieTool.
    /// </summary>
    public sealed partial class GeometryPieTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryPie");
            this._button.Style = this.IconSelectedButtonStyle;

            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPie;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryPieIcon();
        readonly Button _button = new Button { Tag = new GeometryPieIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryPieLayer
            {
                SweepAngle = this.SelectionViewModel.GeometryPieSweepAngle,
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