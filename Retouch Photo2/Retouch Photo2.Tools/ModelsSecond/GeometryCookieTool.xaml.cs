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
    /// Enum of <see cref="GeometryCookieTool">.
    /// </summary>
    internal enum GeometryCookieMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Inner-radius. </summary>
        InnerRadius,

        /// <summary> Sweep-angle. </summary>
        SweepAngle
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public partial class GeometryCookieTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@TouchBar  
        internal GeometryCookieMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryCookieMode.None:
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryCookieMode.InnerRadius:
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.InnerRadiusTouchbarSlider;
                        break;
                    case GeometryCookieMode.SweepAngle:
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.SweepAngleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.SweepAngleTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;

        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);
        private double SweepAngleValueConverter(float sweepAngle) => sweepAngle / System.Math.PI * 180d;


        //@Construct
        public GeometryCookieTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructInnerRadius();
            this.ConstructSweepAngle();
        }


        //InnerRadius
        private void ConstructInnerRadius()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCookieMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryCookieMode.None;
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
            this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCookieLayer geometryCookieLayer)
                {
                    geometryCookieLayer.InnerRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //SweepAngle
        private void ConstructSweepAngle()
        {
            //Button
            this.SweepAngleTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCookieMode.SweepAngle;
                else
                    this.TouchBarMode = GeometryCookieMode.None;
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
            this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCookieLayer geometryCookieLayer)
                {
                    geometryCookieLayer.SweepAngle = sweepAngle;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
     

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryCookieMode.None;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public sealed partial class GeometryCookieTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryCookie");
            this._button.Style = this.IconSelectedButtonStyle;

            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_InnerRadius");
            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCookie;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCookieIcon();
        readonly Button _button = new Button { Tag = new GeometryCookieIcon()};
        
        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCookieLayer
            {
                InnerRadius = this.SelectionViewModel.GeometryCookieInnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryCookieSweepAngle,
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManager()
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}