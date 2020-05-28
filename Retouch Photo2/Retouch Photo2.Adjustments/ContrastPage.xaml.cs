using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentGenericPage<ContrastAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Generic
        public ContrastAdjustment Adjustment { get; set; }

        //@Construct
        public ContrastPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructContrast();
        }
    }

    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentGenericPage<ContrastAdjustment>
    {
        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Contrast");

            this.ContrastTextBlock.Text = resource.GetString("/Adjustments/Contrast_Contrast");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new ContrastAdjustment();


        public void Reset()
        {
            this.ContrastSlider.Value = 0;


            if (this.Adjustment is ContrastAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set contrast adjustment");
                
                var previous = adjustment.Contrast;
                history.UndoActions.Push(() =>
                {
                    ContrastAdjustment adjustment2 = adjustment;

                    adjustment2.Contrast = previous;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.Contrast = 0.0f;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(ContrastAdjustment adjustment)
        {
            this.ContrastSlider.Value = adjustment.Contrast * 100;
        }
    }

    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentGenericPage<ContrastAdjustment>
    {

        public void ConstructContrast()
        {
            this.ContrastSlider.Value = 0;
            this.ContrastSlider.Minimum = -100;
            this.ContrastSlider.Maximum = 100;

            this.ContrastSlider.SliderBrush = this.ContrastBrush;


            //History
            LayersPropertyHistory history = null;


            this.ContrastSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is ContrastAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set contrast adjustment contrast");

                    adjustment.CacheContrast();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.ContrastSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is ContrastAdjustment adjustment)
                {
                    float contrast = (float)value / 100.0f;

                    adjustment.Contrast = contrast;
                    this.ViewModel.Invalidate();
                }
            };
            this.ContrastSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is ContrastAdjustment adjustment)
                {
                    float contrast = (float)value / 100.0f;


                    var previous = adjustment.StartingContrast;
                    history.UndoActions.Push(() =>
                    {
                        ContrastAdjustment adjustment2 = adjustment;

                        adjustment2.Contrast = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Contrast = contrast;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}