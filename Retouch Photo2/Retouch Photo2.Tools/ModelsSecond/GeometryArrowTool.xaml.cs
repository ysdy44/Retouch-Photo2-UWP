using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
    /// Enum of <see cref="GeometryArrowTool"/>.
    /// </summary>
    internal enum GeometryArrowMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Width (IsAbsolute = false). </summary>
        Width,

        /// <summary> Value (IsAbsolute = false). </summary>
        Value
    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public sealed partial class GeometryArrowTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@TouchBar
        internal GeometryArrowMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case GeometryArrowMode.None:
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryArrowMode.Width:
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryArrowMode.Value:
                        this.ValueTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.ValueTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int ValueNumberConverter(float value) => (int)(value * 100.0f);
        private double ValueValueConverter(float value) => value * 100d;
        

        //@Construct
        public GeometryArrowTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructValue1();
            this.ConstructValue2();

            this.ConstructLeftTail();
            this.ConstructRightTail();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = GeometryArrowMode.None;
        }        
    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = 
                this.Title = resource.GetString("/ToolsSecond/GeometryArrow");
            this._button.Style = this.IconSelectedButtonStyle;

            this.ValueTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_LeftTail");

            this.RightTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_RightTail");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryArrow; 
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryArrowIcon();
        readonly Button _button = new Button { Tag = new GeometryArrowIcon()};

        private ILayer CreateLayer(CanvasDevice customDevice, Transformer transformer)
        {
            return new GeometryArrowLayer(customDevice)
            {
                LeftTail = this.SelectionViewModel.GeometryArrowLeftTail,
                RightTail = this.SelectionViewModel.GeometryArrowRightTail,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.GeometryStyle
            };
        }


        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isOutNodeDistance);
        public void Clicke(Vector2 point) => this.TipViewModel.MoveTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }

    /// <summary>
    /// <see cref="ITool"/>'s GeometryArrowTool.
    /// </summary>
    public partial class GeometryArrowTool : Page, ITool
    {

        //Value
        private void ConstructValue1()
        {
            //Button
            this.ValueTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryArrowMode.Value;
                else
                    this.TouchBarMode = GeometryArrowMode.None;
            };

            //Number
            this.ValueTouchbarSlider.Unit = "%";
            this.ValueTouchbarSlider.NumberMinimum = 0;
            this.ValueTouchbarSlider.NumberMaximum = 100;
            this.ValueTouchbarSlider.ValueChanged += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;
                if (value2 < 0.0f) value2 = 0.0f;
                if (value2 > 1.0f) value2 = 1.0f;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set arrow layer value");

                //Selection
                this.SelectionViewModel.GeometryArrowValue = value2;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;

                        var previous = geometryArrowLayer.Value;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryArrowLayer.IsRefactoringRender = true;
                            geometryArrowLayer.IsRefactoringIconRender = true;
                            geometryArrowLayer.Value = previous;
                        });

                        //Refactoring
                        geometryArrowLayer.IsRefactoringRender = true;
                        geometryArrowLayer.IsRefactoringIconRender = true;
                        geometryArrowLayer.Value = value2;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructValue2()
        { 
            //Value
            this.ValueTouchbarSlider.Value = 0;
            this.ValueTouchbarSlider.Minimum = 0;
            this.ValueTouchbarSlider.Maximum = 100;
            this.ValueTouchbarSlider.ValueChangeStarted += (sender, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;
                        geometryArrowLayer.CacheValue();
                    }
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.ValueTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;
                if (value2 < 0.0f) value2 = 0.0f;
                if (value2 > 1.0f) value2 = 1.0f;

                //Selection
                this.SelectionViewModel.GeometryArrowValue = value2;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        geometryArrowLayer.Value = value2;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ValueTouchbarSlider.ValueChangeCompleted += (sender, value) =>
            {
                float value2 = (float)value / 100.0f;
                if (value2 < 0.0f) value2 = 0.0f;
                if (value2 > 1.0f) value2 = 1.0f;
                
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set arrow layer value");

                //Selection
                this.SelectionViewModel.GeometryArrowValue = value2;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;

                        var previous = geometryArrowLayer.StartingValue;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryArrowLayer.IsRefactoringRender = true;
                            geometryArrowLayer.IsRefactoringIconRender = true;
                            geometryArrowLayer.Value = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        geometryArrowLayer.Value = value2;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
               
        //LeftTail
        private void ConstructLeftTail()
        {
            this.LeftArrowTailTypeControl.ArrowTailTypeChanged += (s, tailType) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set arrow layer left tail type");
                
                //Selection
                this.SelectionViewModel.GeometryArrowLeftTail = tailType;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;

                        var previous = geometryArrowLayer.LeftTail;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryArrowLayer.IsRefactoringRender = true;
                            geometryArrowLayer.IsRefactoringIconRender = true;
                            geometryArrowLayer.LeftTail = previous;
                        });

                        //Refactoring
                        geometryArrowLayer.IsRefactoringRender = true;
                        geometryArrowLayer.IsRefactoringIconRender = true;
                        geometryArrowLayer.LeftTail = tailType;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightArrowTailTypeControl.ArrowTailTypeChanged += (s, tailType) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set arrow layer right tail type");

                //Selection
                this.SelectionViewModel.GeometryArrowRightTail = tailType;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;

                        var previous = geometryArrowLayer.RightTail;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            geometryArrowLayer.IsRefactoringRender = true;
                            geometryArrowLayer.IsRefactoringIconRender = true;
                            geometryArrowLayer.RightTail = previous;
                        });

                        //Refactoring
                        geometryArrowLayer.IsRefactoringRender = true;
                        geometryArrowLayer.IsRefactoringIconRender = true;
                        geometryArrowLayer.RightTail = tailType;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}