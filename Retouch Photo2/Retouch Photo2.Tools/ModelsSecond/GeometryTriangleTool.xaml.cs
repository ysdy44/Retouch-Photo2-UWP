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
    /// <see cref="ITool"/>'s GeometryTriangleTool.
    /// </summary>
    public partial class GeometryTriangleTool : Page, ITool
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
            this.ConstructCenter();

            this.MirrorButton.Tapped += (s, e) => this.CenterMirror();
        }


        //Center
        private void ConstructCenter()
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
            this.CenterTouchbarSlider.NumberChange += (sender, number) =>
            {
                float center = number / 100.0f;
                this.CenterChange(center);
            };

            //Value
            this.CenterTouchbarSlider.Minimum = 0d;
            this.CenterTouchbarSlider.Maximum = 100d;
            this.CenterTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.CenterTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float center = (float)value / 100.0f;
                this.CenterChange(center);
            };
            this.CenterTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void CenterChange(float center)
        {
            if (center < 0.0f) center = 0.0f;
            if (center > 1.0f) center = 1.0f;

            this.SelectionViewModel.GeometryTriangleCenter = center;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryTriangleLayer geometryTriangleLayer)
                {
                    geometryTriangleLayer.Center = center;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //Mirror
        private void CenterMirror()
        {
            float selectionCenter = 1.0f - this.SelectionViewModel.GeometryTriangleCenter;
            this.SelectionViewModel.GeometryTriangleCenter = selectionCenter;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryTriangleLayer geometryTriangleLayer)
                {
                    float center = 1.0f - geometryTriangleLayer.Center;
                    geometryTriangleLayer.Center = center;
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
        readonly Button _button = new Button { Tag = new GeometryTriangleIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryTriangleLayer
            {
                Center = this.SelectionViewModel.GeometryTriangleCenter,
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