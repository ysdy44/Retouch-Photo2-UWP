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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

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

            this.ConstructInnerRadius1();
            this.ConstructInnerRadius2();
            this.ConstructSweepAngle1();
            this.ConstructSweepAngle2();
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

            this._button.Content =
                this.Title = resource.GetString("/ToolsSecond/GeometryCookie");
            this._button.Style = this.IconSelectedButtonStyle;

            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_InnerRadius");
            this.SweepAngleTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCookie_SweepAngle");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCookie;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCookieIcon();
        readonly Button _button = new Button { Tag = new GeometryCookieIcon()};
        
        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryCookieLayer(customDevice)
            {
                InnerRadius = this.SelectionViewModel.GeometryCookieInnerRadius,
                SweepAngle = this.SelectionViewModel.GeometryCookieSweepAngle,
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
    /// <see cref="ITool"/>'s GeometryCookieTool.
    /// </summary>
    public sealed partial class GeometryCookieTool : Page, ITool
    {

        //InnerRadius
        private void ConstructInnerRadius1()
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
            this.InnerRadiusTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float innerRadius = (float)value / 100f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set cookie layer inner radius");

                //Selection
                this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;

                        var previous = geometryCookieLayer.InnerRadius;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            geometryCookieLayer.InnerRadius = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryCookieLayer.InnerRadius = innerRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructInnerRadius2()
        {
            //Value
            this.InnerRadiusTouchbarSlider.Value = 0;
            this.InnerRadiusTouchbarSlider.Minimum = 0;
            this.InnerRadiusTouchbarSlider.Maximum = 100;
            this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;
                        geometryCookieLayer.CacheInnerRadius();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float innerRadius = (float)value / 100f;

                //Selection
                this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;
                   
                        //Refactoring
                        geometryCookieLayer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryCookieLayer.InnerRadius = innerRadius;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float innerRadius = (float)value / 100f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set cookie layer inner radius");

                //Selection
                this.SelectionViewModel.GeometryCookieInnerRadius = innerRadius;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;

                        var previous = geometryCookieLayer.StartingInnerRadius;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            geometryCookieLayer.InnerRadius = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryCookieLayer.InnerRadius = innerRadius;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

        //SweepAngle
        private void ConstructSweepAngle1()
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
            this.SweepAngleTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set cookie layer sweep angle");

                //Selection
                this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;

                        var previous = geometryCookieLayer.SweepAngle;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryCookieLayer.IsRefactoringRender = true;
                            geometryCookieLayer.IsRefactoringIconRender = true;
                            geometryCookieLayer.SweepAngle = previous;
                        });

                        //Refactoring
                        geometryCookieLayer.IsRefactoringRender = true;
                        geometryCookieLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        geometryCookieLayer.SweepAngle = sweepAngle;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructSweepAngle2()
        {
            //Value
            this.SweepAngleTouchbarSlider.Value = 0;
            this.SweepAngleTouchbarSlider.Minimum = 0;
            this.SweepAngleTouchbarSlider.Maximum = 360;
            this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;
                        geometryCookieLayer.CacheSweepAngle();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;

                //Selection
                this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        geometryCookieLayer.SweepAngle = sweepAngle;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set cookie layer sweep angle");

                //Selection
                this.SelectionViewModel.GeometryCookieSweepAngle = sweepAngle;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryCookie)
                    {
                        GeometryCookieLayer geometryCookieLayer = (GeometryCookieLayer)layer;

                        var previous = geometryCookieLayer.StartingSweepAngle;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryCookieLayer.IsRefactoringRender = true;
                            geometryCookieLayer.IsRefactoringIconRender = true;
                            geometryCookieLayer.SweepAngle = previous;
                        });

                        //Refactoring
                        geometryCookieLayer.IsRefactoringRender = true;
                        geometryCookieLayer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        geometryCookieLayer.SweepAngle = sweepAngle;
                    }
                });
                
                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }

    }
}