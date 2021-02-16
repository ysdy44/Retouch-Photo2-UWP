// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        
        //@Content
        private float Exposure
        {
            set
            {
                this.ExposurePicker.Value = (int)(value * 100.0f);
                this.ExposureSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ExposurePage. 
        /// </summary>
        public ExposurePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            ExposureAdjustment.GenericText = this.Text;
            ExposureAdjustment.GenericPage = this;

            this.ConstructExposure1();
            this.ConstructExposure2();
        }
    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("Adjustments_Exposure");

            this.ExposureTextBlock.Text = resource.GetString("Adjustments_Exposure_Exposure");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Exposure;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new ExposureIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new ExposureAdjustment();


        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Exposure = 0.0f;

            this.MethodViewModel.TAdjustmentChanged<float, ExposureAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.Exposure = 0,

                type: HistoryType.LayersProperty_ResetAdjustment_Exposure,
                getUndo: (tAdjustment) => tAdjustment.Exposure,
                setUndo: (tAdjustment, previous) => tAdjustment.Exposure = previous
            );
        }
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        public void Follow()
        {
            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (layer.Filter.Adjustments[this.Index] is ExposureAdjustment adjustment)
                {
                    this.Exposure = adjustment.Exposure;
                }
            }
        }

    }

    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentPage
    {

        //Exposure
        private void ConstructExposure1()
        {
            this.ExposurePicker.Unit = "%";
            this.ExposurePicker.Minimum = -200;
            this.ExposurePicker.Maximum = 200;
            this.ExposurePicker.ValueChanged += (s, value) =>
            {
                float exposure = (float)value / 100.0f;
                this.Exposure = exposure;

                this.MethodViewModel.TAdjustmentChanged<float, ExposureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Exposure = exposure,

                    type: HistoryType.LayersProperty_SetAdjustment_Exposure_Exposure,
                    getUndo: (tAdjustment) => tAdjustment.Exposure,
                    setUndo: (tAdjustment, previous) => tAdjustment.Exposure = previous
                );
            };
        }
        private void ConstructExposure2()
        {
            this.ExposureSlider.SliderBrush = this.ExposureBrush;
            this.ExposureSlider.Minimum = -2.0d;
            this.ExposureSlider.Maximum = 2.0d;
            this.ExposureSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<ExposureAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheExposure());
            this.ExposureSlider.ValueChangeDelta += (s, value) =>
            {
                float exposure = (float)value;
                this.Exposure = exposure;

                this.MethodViewModel.TAdjustmentChangeDelta<ExposureAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Exposure = exposure);
            };
            this.ExposureSlider.ValueChangeCompleted += (s, value) =>
            {
                float exposure = (float)value;
                this.Exposure = exposure;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, ExposureAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Exposure = exposure,

                    type: HistoryType.LayersProperty_SetAdjustment_Exposure_Exposure,
                    getUndo: (tAdjustment) => tAdjustment.StartingExposure,
                    setUndo: (tAdjustment, previous) => tAdjustment.Exposure = previous
                );
            };
        }

    }
}