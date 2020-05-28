using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Generic
        public SaturationAdjustment Adjustment { get; set; }

        //@Construct
        public SaturationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructSaturation();
        }
    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Saturation");

            this.SaturationTextBlock.Text = resource.GetString("/Adjustments/Saturation_Saturation");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new SaturationAdjustment();

        public void Reset()
        {
            this.SaturationSlider.Value = 100;


            if (this.Adjustment is SaturationAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set saturation adjustment");
                
                var previous = adjustment.Saturation;
                history.UndoActions.Push(() =>
                {
                    SaturationAdjustment adjustment2 = adjustment;

                    adjustment2.Saturation = previous;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.Saturation = 1.0f;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(SaturationAdjustment adjustment)
        {
            this.SaturationSlider.Value = adjustment.Saturation * 100.0f;
        }
    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentGenericPage<SaturationAdjustment>
    {

        public void ConstructSaturation()
        {
            this.SaturationSlider.Value = 100;
            this.SaturationSlider.Minimum = 0;
            this.SaturationSlider.Maximum = 200;

            this.SaturationSlider.SliderBrush = this.SaturationBrush;


            //History
            LayersPropertyHistory history = null;


            this.SaturationSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is SaturationAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set saturation adjustment saturation");

                    adjustment.CacheSaturation();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is SaturationAdjustment adjustment)
                {
                    float saturation = (float)value / 100.0f;

                    adjustment.Saturation = saturation;
                    this.ViewModel.Invalidate();
                }
            };
            this.SaturationSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is SaturationAdjustment adjustment)
                {
                    float saturation = (float)value / 100.0f;


                    var previous = adjustment.StartingSaturation;
                    history.UndoActions.Push(() =>
                    {
                        SaturationAdjustment adjustment2 = adjustment;

                        adjustment2.Saturation = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Saturation = saturation;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}