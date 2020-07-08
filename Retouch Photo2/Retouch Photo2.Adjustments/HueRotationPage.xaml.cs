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
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        private float Angle
        {
            set
            {
                this.AnglePicker.Value = (int)(value * 180.0f / FanKit.Math.Pi);
                this.AngleSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a HueRotationPage. 
        /// </summary>
        public HueRotationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            HueRotationAdjustment.GenericText = this.Text;
            HueRotationAdjustment.GenericPage = this;

            this.ConstructHueRotation1();
            this.ConstructHueRotation2();
        }
    }

    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HueRotation");

            this.AngleTextBlock.Text = resource.GetString("/Adjustments/HueRotation_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.HueRotation;
        /// <summary> Gets the icon. </summary>
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Text { get; private set; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        public IAdjustment GetNewAdjustment() => new HueRotationAdjustment();
    
        
        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Angle = 0.0f;

            this.MethodViewModel.TAdjustmentChanged<float, HueRotationAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.Angle = 0,

                historyTitle: "Set hue rotation adjustment",
                getHistory: (tAdjustment) => tAdjustment.Angle,
                setHistory: (tAdjustment, previous) => tAdjustment.Angle = previous
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

                if (layer.Filter.Adjustments[this.Index] is HueRotationAdjustment adjustment)
                {
                    this.Angle = adjustment.Angle;
                }
            }
        }

    }

    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentPage
    {

        //HueRotation
        private void ConstructHueRotation1()
        {
            this.AnglePicker.Unit = "º";
            this.AnglePicker.Minimum = 0;
            this.AnglePicker.Maximum = 360;
            this.AnglePicker.ValueChanged += (s, value) =>
            {
                float radians = (float)value * FanKit.Math.Pi / 180.0f;
                this.Angle = radians;

                this.MethodViewModel.TAdjustmentChanged<float, HueRotationAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Angle = radians,

                    historyTitle: "Set hue rotation adjustment angle",
                    getHistory: (tAdjustment) => tAdjustment.Angle,
                    setHistory: (tAdjustment, previous) => tAdjustment.Angle = previous
                );
            };
        }

        private void ConstructHueRotation2()
        {
            this.AngleSlider.SliderBrush = this.AngleBrush;
            this.AngleSlider.Minimum = 0.0d;
            this.AngleSlider.Maximum = FanKit.Math.PiTwice;
            this.AngleSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<HueRotationAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheAngle());
            this.AngleSlider.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.TAdjustmentChangeDelta<HueRotationAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Angle = radians);
            };
            this.AngleSlider.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, HueRotationAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Angle = radians,

                    historyTitle: "Set hue rotation adjustment angle",
                    getHistory: (tAdjustment) => tAdjustment.StartingAngle,
                    setHistory: (tAdjustment, previous) => tAdjustment.Angle = previous
                );
            };
        }

    }
}