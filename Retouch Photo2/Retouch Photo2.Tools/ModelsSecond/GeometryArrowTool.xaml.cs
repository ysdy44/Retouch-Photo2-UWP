using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
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
            this.ConstructValue();
            this.ConstructLeftTail();
            this.ConstructRightTail();
        }


        //Value
        private void ConstructValue()
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
            this.ValueTouchbarSlider.NumberChange += (sender, number) =>
            {
                float value2 = number / 100f;
                this.ValueChange(value2);
            };

            //Value
            this.ValueTouchbarSlider.Minimum = 0d;
            this.ValueTouchbarSlider.Maximum = 100d;
            this.ValueTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.ValueTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float value2 = (float)(value / 100d);
                this.ValueChange(value2);
            };
            this.ValueTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void ValueChange(float value)
        {
            this.SelectionViewModel.GeometryArrowValue = value;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer.Type == LayerType.GeometryArrow)
                {
                    GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;
                    geometryArrowLayer.Value = value;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //LeftTail
        private void ConstructLeftTail()
        {
            this.LeftArrowTailTypeControl.ArrowTailTypeChanged += (s, tailType) =>
            {
                this.SelectionViewModel.GeometryArrowLeftTail = tailType;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;
                        geometryArrowLayer.LeftTail = tailType;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //RightTail
        private void ConstructRightTail()
        {
            this.RightArrowTailTypeControl.ArrowTailTypeChanged += (s, tailType) =>
            {
                this.SelectionViewModel.GeometryArrowRightTail = tailType;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer.Type == LayerType.GeometryArrow)
                    {
                        GeometryArrowLayer geometryArrowLayer = (GeometryArrowLayer)layer;
                        geometryArrowLayer.RightTail = tailType;
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
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

            this._button.Content = resource.GetString("/ToolsSecond/GeometryArrow");
            this._button.Style = this.IconSelectedButtonStyle;

            this.ValueTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryArrow_Value");

            this.LeftTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_LeftTail");

            this.RightTailTextBlock.Text = resource.GetString("/ToolsSecond/GeometryArrow_RightTail");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryArrow;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryArrowIcon();
        readonly Button _button = new Button { Tag = new GeometryArrowIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryArrowLayer
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
        public void Clicke(Vector2 point) => this.TipViewModel.TransformerTool.Clicke(point);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}