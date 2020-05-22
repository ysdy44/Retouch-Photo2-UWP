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
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public partial class GeometryPentagonTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar  
        internal bool TouchBarMode
        {
            set
            {
                if (value == false)
                {
                    this.PointsTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.PointsTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.PointsTouchbarSlider;
                }
            }
        }


        //@Converter
        private double PointsValueConverter(float points) => points;


        //@Construct
        public GeometryPentagonTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructPoints();
        }


        //Points
        private void ConstructPoints()
        {
            //Button
            this.PointsTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.PointsTouchbarSlider.Unit = "";
            this.PointsTouchbarSlider.NumberMinimum = 0;
            this.PointsTouchbarSlider.NumberMaximum = 100;
            this.PointsTouchbarSlider.NumberChange += (sender, number) =>
            {
                int Points = number;
                this.PointsChange(Points);
            };

            //Value
            this.PointsTouchbarSlider.Minimum = 0d;
            this.PointsTouchbarSlider.Maximum = 100d;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                int Points = (int)value;
                this.PointsChange(Points);
            };
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void PointsChange(int points)
        {
            if (points < 3) points = 3;
            if (points > 36) points = 36;

            this.ViewModel.GeometryPentagonPoints = points;

            //Selection
            this.ViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.GeometryPentagon)
                {
                    GeometryPentagonLayer geometryPentagonLayer = (GeometryPentagonLayer)layer;
                    geometryPentagonLayer.Points = points;
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
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public sealed partial class GeometryPentagonTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryPentagon");
            this._button.Style = this.IconSelectedButtonStyle;

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPentagon_Points");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPentagon;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryPentagonIcon();
        readonly Button _button = new Button { Tag = new GeometryPentagonIcon()};
        
        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryPentagonLayer
            {
                Transform = new Transform(transformer),
                Style = this.ViewModel.GeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}