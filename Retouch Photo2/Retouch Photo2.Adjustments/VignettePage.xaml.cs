using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        public Color Color
        {
            get => this.SolidColorBrush.Color;
            set
            {
                this.SolidColorBrush.Color = value;
                this.AmountRight.Color = value;
                this.CurveRight.Color = value;
            }
        }

        //@Generic
        public VignetteAdjustment Adjustment { get; set; }
        
        //@Construct
        public VignettePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructAmount();
            this.ConstructCurve();

            this.ConstructColor1();
            this.ConstructColor2();

            //TODO
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                this.Color = value;

                if (this.Adjustment == null) return;

                this.Adjustment.Color = value;
                this.ViewModel.Invalidate();
            };
        }
    }

    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Vignette");

            this.AmountTextBlock.Text = resource.GetString("/Adjustments/Vignette_Amount");
            this.CurveTextBlock.Text = resource.GetString("/Adjustments/Vignette_Curve");
            this.ColorTextBlock.Text = resource.GetString("/Adjustments/Vignette_Color");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new VignetteAdjustment();


        public void Reset()
        {
            this.AmountSlider.Value = 0;
            this.CurveSlider.Value = 0;
            this.Color = Colors.Black;


            if (this.Adjustment is VignetteAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment");


                var previous1 = adjustment.Amount;
                var previous2 = adjustment.Curve;
                var previous3 = adjustment.Color;
                history.UndoActions.Push(() =>
                {
                    VignetteAdjustment adjustment2 = adjustment;

                    adjustment2.Amount = previous1;
                    adjustment2.Curve = previous2;
                    adjustment2.Color = previous3;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.Amount = 0.0f;
                adjustment.Curve = 0.0f;
                adjustment.Color = Colors.Black;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(VignetteAdjustment adjustment)
        {
            this.AmountSlider.Value = adjustment.Amount * 100;
            this.CurveSlider.Value = adjustment.Curve * 100;
            this.Color = adjustment.Color;
        }
    }

    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentGenericPage<VignetteAdjustment>
    {
        
        public void ConstructAmount()
        {
            this.AmountSlider.Value = 0;
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 100;

            this.AmountSlider.SliderBrush = this.AmountBrush;


            //History
            LayersPropertyHistory history = null;


            this.AmountSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set vignette adjustment amount");

                    adjustment.CacheAmount();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    float amount = (float)value / 100.0f;

                    adjustment.Amount = amount;
                    this.ViewModel.Invalidate();
                }
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    float amount = (float)value / 100.0f;


                    var previous = adjustment.StartingAmount;
                    history.UndoActions.Push(() =>
                    {
                        VignetteAdjustment adjustment2 = adjustment;

                        adjustment2.Amount = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Amount = amount;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

        public void ConstructCurve()
        {
            this.CurveSlider.Value = 0;
            this.CurveSlider.Minimum = 0;
            this.CurveSlider.Maximum = 100;

            this.CurveSlider.SliderBrush = this.CurveBrush;


            //History
            LayersPropertyHistory history = null;


            this.CurveSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set vignette adjustment curve");

                    adjustment.CacheCurve();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    float curve = (float)value / 100.0f;

                    adjustment.Curve = curve;
                    this.ViewModel.Invalidate();
                }
            };
            this.CurveSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    float curve = (float)value / 100.0f;


                    var previous = adjustment.StartingCurve;
                    history.UndoActions.Push(() =>
                    {
                        VignetteAdjustment adjustment2 = adjustment;

                        adjustment2.Curve = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Curve = curve;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructColor1()
        {
            this.ColorBorder.Tapped += (s, e) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    this.ColorPicker.Color = adjustment.Color;
                    this.ColorFlyout.ShowAt(this.ColorBorder);
                }
            };

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    Color color = value;
                    this.Color = color;

                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set vignette adjustment color");

                    var previous = adjustment.Color;
                    history.UndoActions.Push(() =>
                    {
                        VignetteAdjustment adjustment2 = adjustment;

                        adjustment2.Color = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Color = color;
                    this.ViewModel.Invalidate();
                }
            };
        }

        public void ConstructColor2()
        {
            //History
            LayersPropertyHistory history = null;


            this.ColorPicker.ColorChangeStarted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set vignette adjustment color");

                    adjustment.CacheColor();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    Color color = value;

                    adjustment.Color = color;
                    this.ViewModel.Invalidate();
                }
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is VignetteAdjustment adjustment)
                {
                    Color color = value;


                    var previous = adjustment.StartingColor;
                    history.UndoActions.Push(() =>
                    {
                        VignetteAdjustment adjustment2 = adjustment;

                        adjustment2.Color = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Color = color;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}