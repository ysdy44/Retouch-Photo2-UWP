using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Generic
        public ExposureAdjustment Adjustment { get; set; }

        //@Construct
        public ExposurePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructExposure();
        }
    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Exposure");

            this.ExposureTextBlock.Text = resource.GetString("/Adjustments/Exposure_Exposure");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new ExposureAdjustment();


        public void Reset()
        {
            this.ExposureSlider.Value = 0;


            if (this.Adjustment is ExposureAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set contrast adjustment");
                
                var previous = adjustment.Exposure;
                history.UndoActions.Push(() =>
                {
                    ExposureAdjustment adjustment2 = adjustment;

                    adjustment2.Exposure = previous;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.Exposure = 0.0f;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(ExposureAdjustment adjustment)
        {
            this.ExposureSlider.Value = adjustment.Exposure * 100;
        }
    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentGenericPage<ExposureAdjustment>
    {

        public void ConstructExposure()
        {
            this.ExposureSlider.Value = 0;
            this.ExposureSlider.Minimum = -200;
            this.ExposureSlider.Maximum = 200;

            this.ExposureSlider.SliderBrush = this.ExposureBrush;


            //History
            LayersPropertyHistory history = null;


            this.ExposureSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is ExposureAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set exposure adjustment exposure");

                    adjustment.CacheExposure();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.ExposureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is ExposureAdjustment adjustment)
                {
                    float exposure = (float)value / 100.0f;

                    adjustment.Exposure = exposure;
                    this.ViewModel.Invalidate();
                }
            };
            this.ExposureSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is ExposureAdjustment adjustment)
                {
                    float exposure = (float)value / 100.0f;


                    var previous = adjustment.StartingExposure;
                    history.UndoActions.Push(() =>
                    {
                        ExposureAdjustment adjustment2 = adjustment;

                        adjustment2.Exposure = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Exposure = exposure;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}