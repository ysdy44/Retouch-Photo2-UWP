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
    internal enum AcrylicMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Tint-opacity. </summary>
        TintOpacity,

        /// <summary> Tint-amount. </summary>
        BlurAmount,
    }

    /// <summary> 
    /// <see cref = "ITool"/> of AcrylicTool.
    /// </summary>
    public sealed partial class AcrylicTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@TouchBar
        internal AcrylicMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case AcrylicMode.None:
                        this.TintOpacityTouchbarButton.IsSelected = false;
                        this.BlurAmountTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case AcrylicMode.TintOpacity:
                        this.TintOpacityTouchbarButton.IsSelected = true;
                        this.BlurAmountTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.TintOpacityTouchbarSlider;
                        break;
                    case AcrylicMode.BlurAmount:
                        this.TintOpacityTouchbarButton.IsSelected = false;
                        this.BlurAmountTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.BlurAmountTouchbarSlider;
                        break;
                }
            }
        }


        //@Converter
        private int BlurAmountNumberConverter(float blurAmount) => (int)blurAmount;
        private double BlurAmountValueConverter(float blurAmount) => blurAmount;
        
        private int TintOpacityNumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private double TintOpacityValueConverter(float tintOpacity) => tintOpacity * 100d;


        //@Construct
        public AcrylicTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructBlurAmount();
            this.ConstructTintOpacity();
        }


        //BlurAmount
        private void ConstructBlurAmount()
        {
            //Button
            this.BlurAmountTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = AcrylicMode.BlurAmount;
                else
                    this.TouchBarMode = AcrylicMode.None;
            };

            //Number
            this.BlurAmountTouchbarSlider.Unit = "dp";
            this.BlurAmountTouchbarSlider.NumberMinimum = 10;
            this.BlurAmountTouchbarSlider.NumberMaximum = 100;
            this.BlurAmountTouchbarSlider.NumberChange += (sender, number) =>
            {
                float amount = number;
                this.BlurAmountChange(amount);
            };

            //Value
            this.BlurAmountTouchbarSlider.Minimum = 10d;
            this.BlurAmountTouchbarSlider.Maximum = 100d;
            this.BlurAmountTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.BlurAmountTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float amount = (float)value;
                this.BlurAmountChange(amount);
            };
            this.BlurAmountTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void BlurAmountChange(float amount)
        {
            if (amount < 10.0f) amount = 10.0f;
            if (amount > 100.0f) amount = 100.0f;

            this.SelectionViewModel.AcrylicBlurAmount = amount;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.BlurAmount = amount;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        
        //TintOpacity
        private void ConstructTintOpacity()
        {
            //Button
            this.TintOpacityTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = AcrylicMode.TintOpacity;
                else
                    this.TouchBarMode = AcrylicMode.None;
            };

            //Number
            this.TintOpacityTouchbarSlider.Unit = "%";
            this.TintOpacityTouchbarSlider.NumberMinimum = 0;
            this.TintOpacityTouchbarSlider.NumberMaximum = 90;
            this.TintOpacityTouchbarSlider.NumberChange += (sender, number) =>
            {
                float tintOpacity = number / 100f;
                this.TintOpacityChange(tintOpacity);
            };

            //Value
            this.TintOpacityTouchbarSlider.Minimum = 0d;
            this.TintOpacityTouchbarSlider.Maximum = 90d;
            this.TintOpacityTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TintOpacityTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float tintOpacity = (float)(value / 100d);
                this.TintOpacityChange(tintOpacity);
            };
            this.TintOpacityTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void TintOpacityChange(float opacity)
        {
            this.SelectionViewModel.AcrylicTintOpacity = opacity;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.TintOpacity = opacity;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = AcrylicMode.None;
        }

    }

    /// <summary>
    /// <see cref = "ITool"/> of AcrylicTool.
    /// </summary>
    public sealed partial class AcrylicTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Acrylic");

            this.TintColorTextBlock.Text = resource.GetString("/Tools/Acrylic_TintColor");
            this.TintOpacityTouchbarButton.CenterContent = resource.GetString("/Tools/Acrylic_TintOpacity");
            this.BlurAmountTouchbarButton.CenterContent = resource.GetString("/Tools/Acrylic_BlurAmount");
        }


        //@Content
        public ToolType Type => ToolType.Acrylic;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new AcrylicIcon();
        readonly ToolButton _button = new ToolButton(new AcrylicIcon());

        readonly CreateTool CreateTool = new CreateTool
        {
            CreateLayer = (Transformer transformer) =>
            {
                return new AcrylicLayer
                {
                    SelectMode = SelectMode.Selected,
                    TransformManager = new TransformManager(transformer)
                    {
                        DisabledRadian = true//DisabledRadian
                    },
                };
            }
        };


        public void Starting(Vector2 point) => this.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.CreateTool.Started(startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.CreateTool.Draw(drawingSession);

    }
}