using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Generic
        public BrightnessAdjustment Adjustment { get; set; }

        //@Construct
        public BrightnessPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructWhiteLight();
            this.ConstructWhiteDark();

            this.ConstructBlackLight();
            this.ConstructBlackDark();
        }
    }

    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    {

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Brightness");

            this.WhiteToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToLight");
            this.WhiteToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToDark");

            this.BlackToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToLight");
            this.BlackToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToDark");
        }


        //@Content
        public AdjustmentType Type => AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new BrightnessAdjustment();


        public void Reset()
        {
            this.WhiteLightSlider.Value = 100;
            this.WhiteDarkSlider.Value = 100;

            this.BlackLightSlider.Value = 0;
            this.BlackDarkSlider.Value = 0;


            if (this.Adjustment is BrightnessAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment");
                
                var previous1 = adjustment.WhiteLight;
                var previous2 = adjustment.WhiteDark;
                var previous3 = adjustment.BlackLight;
                var previous4 = adjustment.BlackDark;
                history.UndoActions.Push(() =>
                {
                    BrightnessAdjustment adjustment2 = adjustment;

                    adjustment2.WhiteLight = previous1;
                    adjustment2.WhiteDark = previous2;
                    adjustment2.BlackLight = previous3;
                    adjustment2.BlackDark = previous4;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.WhiteLight = 1.0f;
                adjustment.WhiteDark = 1.0f;
                adjustment.BlackLight = 0.0f;
                adjustment.BlackDark = 0.0f;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(BrightnessAdjustment adjustment)
        {
            this.WhiteLightSlider.Value = adjustment.WhiteLight * 100;
            this.WhiteDarkSlider.Value = adjustment.WhiteDark * 100;

            this.BlackLightSlider.Value = adjustment.BlackLight * 100;
            this.BlackDarkSlider.Value = adjustment.BlackDark * 100;
        }

    }

    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    { 

        public void ConstructWhiteLight()
        {
            this.WhiteLightSlider.Value = 100;
            this.WhiteLightSlider.Minimum = 50;
            this.WhiteLightSlider.Maximum = 100;

            this.WhiteLightSlider.SliderBrush = this.WhiteLightBrush;


            //History
            LayersPropertyHistory history = null;


            this.WhiteLightSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set brightness adjustment white light");

                    adjustment.CacheWhiteLight();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.WhiteLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float light = (float)value / 100.0f;

                    adjustment.WhiteLight = light;
                    this.ViewModel.Invalidate();
                }
            };
            this.WhiteLightSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float light = (float)value / 100.0f;


                    var previous = adjustment.StartingWhiteLight;
                    history.UndoActions.Push(() =>
                    {
                        BrightnessAdjustment adjustment2 = adjustment;

                        adjustment2.WhiteLight = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.WhiteLight = light;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

        public void ConstructWhiteDark()
        {
            this.WhiteDarkSlider.Value = 100;
            this.WhiteDarkSlider.Minimum = 50;
            this.WhiteDarkSlider.Maximum = 100;

            this.WhiteDarkSlider.SliderBrush = this.WhiteDarkBrush;


            //History
            LayersPropertyHistory history = null;


            this.WhiteDarkSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set brightness adjustment white dark");

                    adjustment.CacheWhiteDark();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.WhiteDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float dark = (float)value / 100.0f;

                    adjustment.WhiteDark = dark;
                    this.ViewModel.Invalidate();
                }
            };
            this.WhiteDarkSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float dark = (float)value / 100.0f;


                    var previous = adjustment.StartingWhiteDark;
                    history.UndoActions.Push(() =>
                    {
                        BrightnessAdjustment adjustment2 = adjustment;

                        adjustment2.WhiteDark = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.WhiteDark = dark;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructBlackLight()
        {
            this.BlackLightSlider.Value = 0;
            this.BlackLightSlider.Minimum = 0;
            this.BlackLightSlider.Maximum = 50;

            this.BlackLightSlider.SliderBrush = this.BlackLightBrush;


            //History
            LayersPropertyHistory history = null;


            this.BlackLightSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set brightness adjustment black light");

                    adjustment.CacheBlackLight();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.BlackLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float light = (float)value / 100.0f;

                    adjustment.BlackLight = light;
                    this.ViewModel.Invalidate();
                }
            };
            this.BlackLightSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float light = (float)value / 100.0f;


                    var previous = adjustment.StartingBlackLight;
                    history.UndoActions.Push(() =>
                    {
                        BrightnessAdjustment adjustment2 = adjustment;

                        adjustment2.BlackLight = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlackLight = light;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

        public void ConstructBlackDark()
        {
            this.BlackDarkSlider.Value = 0;
            this.BlackDarkSlider.Minimum = 0;
            this.BlackDarkSlider.Maximum = 50;

            this.BlackDarkSlider.SliderBrush = this.BlackDarkBrush;


            //History
            LayersPropertyHistory history = null;


            this.BlackDarkSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set brightness adjustment black dark");

                    adjustment.CacheBlackDark();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.BlackDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float dark = (float)value / 100.0f;

                    adjustment.BlackDark = dark;
                    this.ViewModel.Invalidate();
                }
            };
            this.BlackDarkSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    float dark = (float)value / 100.0f;


                    var previous = adjustment.StartingBlackDark;
                    history.UndoActions.Push(() =>
                    {
                        BrightnessAdjustment adjustment2 = adjustment;

                        adjustment2.BlackDark = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlackDark = dark;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}