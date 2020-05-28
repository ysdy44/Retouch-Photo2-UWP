using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Generic
        public TemperatureAdjustment Adjustment { get; set; }

        //@Construct
        public TemperaturePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructTemperature();
            this.ConstructTint();
        }
    }

    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Temperature");

            this.TemperatureTextBlock.Text = resource.GetString("/Adjustments/Temperature_Temperature");
            this.TintTextBlock.Text = resource.GetString("/Adjustments/Temperature_Tint");
        }

        //@Content
        public AdjustmentType Type => AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new TemperatureAdjustment();

        public void Reset()
        {
            this.TemperatureSlider.Value = 0;
            this.TintSlider.Value = 0;


            if (this.Adjustment is TemperatureAdjustment adjustment)
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set temperature adjustment");
                
                var previous1 = adjustment.Temperature;
                var previous2 = adjustment.Tint;
                history.UndoActions.Push(() =>
                {
                    TemperatureAdjustment adjustment2 = adjustment;

                    adjustment2.Temperature = previous1;
                    adjustment2.Tint = previous2;
                });

                this.ViewModel.HistoryPush(history);


                adjustment.Temperature = 0.0f;
                adjustment.Tint = 0.0f;

                this.ViewModel.Invalidate();
            }
        }
        public void Follow(TemperatureAdjustment adjustment)
        {
            this.TemperatureSlider.Value = adjustment.Temperature * 100;
            this.TintSlider.Value = adjustment.Tint * 100;
        }
    }

    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentGenericPage<TemperatureAdjustment>
    {
        
        public void ConstructTemperature()
        {
            this.TemperatureSlider.Value = 0;
            this.TemperatureSlider.Minimum = -100;
            this.TemperatureSlider.Maximum = 100;

            this.TemperatureSlider.SliderBrush = this.TemperatureBrush;


            //History
            LayersPropertyHistory history = null;


            this.TemperatureSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set temperature adjustment temperature");

                    adjustment.CacheTemperature();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    float temperature = (float)value / 100.0f;

                    adjustment.Temperature = temperature;
                    this.ViewModel.Invalidate();
                }
            };
            this.TemperatureSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    float temperature = (float)value / 100.0f;


                    var previous = adjustment.StartingTemperature;
                    history.UndoActions.Push(() =>
                    {
                        TemperatureAdjustment adjustment2 = adjustment;

                        adjustment2.Temperature = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Temperature = temperature;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

        public void ConstructTint()
        {
            this.TintSlider.Value = 0;
            this.TintSlider.Minimum = -100;
            this.TintSlider.Maximum = 100;

            this.TintSlider.SliderBrush = this.TintBrush;


            //History
            LayersPropertyHistory history = null;


            this.TintSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set temperature adjustment tint");

                    adjustment.CacheTint();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    float tint = (float)value / 100.0f;

                    adjustment.Tint = tint;
                    this.ViewModel.Invalidate();
                }
            };
            this.TintSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is TemperatureAdjustment adjustment)
                {
                    float tint = (float)value / 100.0f;


                    var previous = adjustment.StartingTint;
                    history.UndoActions.Push(() =>
                    {
                        TemperatureAdjustment adjustment2 = adjustment;

                        adjustment2.Tint = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.Tint = tint;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}