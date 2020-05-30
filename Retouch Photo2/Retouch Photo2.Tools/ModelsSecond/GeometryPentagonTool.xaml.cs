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
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public partial class GeometryPentagonTool : Page, ITool
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

            this.ConstructPoints1();
            this.ConstructPoints2();
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

            this._button.Content = 
                this.Title = resource.GetString("/ToolsSecond/GeometryPentagon");
            this._button.Style = this.IconSelectedButtonStyle;

            this.PointsTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryPentagon_Points");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryPentagon;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryPentagonIcon();
        readonly Button _button = new Button { Tag = new GeometryPentagonIcon()};
        
        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryPentagonLayer(customDevice)
            {
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandGeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryPentagonTool.
    /// </summary>
    public sealed partial class GeometryPentagonTool : Page, ITool
    {
        //Points
        private void ConstructPoints1()
        {
            //Button
            this.PointsTouchbarButton.Toggle += (s, value) =>
            {
                this.TouchBarMode = value;
            };

            //Number
            this.PointsTouchbarSlider.NumberMinimum = 3;
            this.PointsTouchbarSlider.NumberMaximum = 36;
            this.PointsTouchbarSlider.ValueChanged += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set pentagon layer points");

                //Selection
                this.SelectionViewModel.GeometryPentagonPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryPentagon)
                    {
                        GeometryPentagonLayer geometryPentagonLayer = (GeometryPentagonLayer)layer;

                        var previous = geometryPentagonLayer.Points;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryPentagonLayer.IsRefactoringRender = true;
                            geometryPentagonLayer.IsRefactoringIconRender = true;
                            geometryPentagonLayer.Points = previous;
                        });

                        //Refactoring
                        geometryPentagonLayer.IsRefactoringRender = true;
                        geometryPentagonLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        geometryPentagonLayer.Points = points;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructPoints2()
        {
            //Value
            this.PointsTouchbarSlider.Value = 3;
            this.PointsTouchbarSlider.Minimum = 3;
            this.PointsTouchbarSlider.Maximum = 36;
            this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryPentagon)
                    {
                        GeometryPentagonLayer geometryPentagonLayer = (GeometryPentagonLayer)layer;
                        geometryPentagonLayer.CachePoints();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //Selection
                this.SelectionViewModel.GeometryPentagonPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryPentagon)
                    {
                        GeometryPentagonLayer geometryPentagonLayer = (GeometryPentagonLayer)layer;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryPentagonLayer.Points = points;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                int points = (int)value;
                if (points < 3) points = 3;
                if (points > 36) points = 36;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set pentagon layer points");

                //Selection
                this.SelectionViewModel.GeometryPentagonPoints = points;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryPentagon)
                    {
                        GeometryPentagonLayer geometryPentagonLayer = (GeometryPentagonLayer)layer;

                        var previous = geometryPentagonLayer.StartingPoints;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            geometryPentagonLayer.Points = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        geometryPentagonLayer.Points = points;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}