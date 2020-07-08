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
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        private float Saturation
        {
            set
            {
                this.SaturationPicker.Value = (int)(value * 100.0f);
                this.SaturationSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a SaturationPage. 
        /// </summary>
        public SaturationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            SaturationAdjustment.GenericText = this.Text;
            SaturationAdjustment.GenericPage = this;

            this.ConstructSaturation1();
            this.ConstructSaturation2();
        }
    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentPage
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Saturation");

            this.SaturationTextBlock.Text = resource.GetString("/Adjustments/Saturation_Saturation");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Saturation;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new SaturationIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new SaturationAdjustment();


        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Saturation = 1.0f;

            this.MethodViewModel.TAdjustmentChanged<float, SaturationAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.Saturation = 0,

                historyTitle: "Set saturation adjustment",
                getHistory: (tAdjustment) => tAdjustment.Saturation,
                setHistory: (tAdjustment, previous) => tAdjustment.Saturation = previous
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

                if (layer.Filter.Adjustments[this.Index] is SaturationAdjustment adjustment)
                {
                    this.Saturation = adjustment.Saturation;
                }
            }
        }

    }

    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentPage
    {

        //Saturation
        private void ConstructSaturation1()
        {
            this.SaturationPicker.Unit = null;
            this.SaturationPicker.Minimum = 0;
            this.SaturationPicker.Maximum = 200;
            this.SaturationPicker.ValueChanged += (s, value) =>
            {
                float saturation = (float)value / 100.0f;
                this.Saturation = saturation;

                this.MethodViewModel.TAdjustmentChanged<float, SaturationAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Saturation = saturation,

                    historyTitle: "Set saturation adjustment saturation",
                    getHistory: (tAdjustment) => tAdjustment.Saturation,
                    setHistory: (tAdjustment, previous) => tAdjustment.Saturation = previous
                );
            };
        }

        private void ConstructSaturation2()
        {
            this.SaturationSlider.Minimum = 0.0d;
            this.SaturationSlider.Maximum = 2.0d;
            this.SaturationSlider.SliderBrush = this.SaturationBrush;
            this.SaturationSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<SaturationAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheSaturation());
            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                float saturation = (float)value;
                this.Saturation = saturation;

                this.MethodViewModel.TAdjustmentChangeDelta<SaturationAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Saturation = saturation);
            };
            this.SaturationSlider.ValueChangeCompleted += (s, value) =>
            {
                float saturation = (float)value;
                this.Saturation = saturation;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, SaturationAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Saturation = saturation,

                    historyTitle: "Set saturation adjustment saturation",
                    getHistory: (tAdjustment) => tAdjustment.StartingSaturation,
                    setHistory: (tAdjustment, previous) => tAdjustment.Saturation = previous
                );
            };
        }

    }
}