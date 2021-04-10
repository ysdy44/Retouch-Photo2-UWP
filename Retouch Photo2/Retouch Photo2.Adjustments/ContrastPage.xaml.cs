// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              
// Complete:      ★★★
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentPage
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public AdjustmentType Type => AdjustmentType.Contrast;
        /// <summary> Gets the icon. </summary>
        public ControlTemplate Icon => this.IconContentControl.Template;
        /// <summary> Gets the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Gets the text. </summary>
        public string Title { get; private set; }

        /// <summary> Gets the adjustment index. </summary>
        public int Index { get; set; }

        private float Contrast
        {
            set
            {
                this.ContrastPicker.Value = (int)(value * 100.0f);
                this.ContrastSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a ContrastPage. 
        /// </summary>
        public ContrastPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructContrast1();
            this.ConstructContrast2();
        }
    }

    public sealed partial class ContrastPage : IAdjustmentPage
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Adjustments_Contrast");

            this.ContrastTextBlock.Text = resource.GetString("Adjustments_Contrast_Contrast");
        }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        public void Reset()
        {
            this.Contrast = 0.0f;

            this.MethodViewModel.TAdjustmentChanged<float, ContrastAdjustment>
            (
                index: this.Index,
                set: (tAdjustment) => tAdjustment.Contrast = 0,

                type: HistoryType.LayersProperty_ResetAdjustment_Contrast,
                getUndo: (tAdjustment) => tAdjustment.Contrast,
                setUndo: (tAdjustment, previous) => tAdjustment.Contrast = previous
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

                if (layer.Filter.Adjustments[this.Index] is ContrastAdjustment adjustment)
                {
                    this.Contrast = adjustment.Contrast;
                }
            }
        }

    }

    public sealed partial class ContrastPage : IAdjustmentPage
    {

        //Contrast
        private void ConstructContrast1()
        {
            this.ContrastPicker.Unit = "%";
            this.ContrastPicker.Minimum = -100;
            this.ContrastPicker.Maximum = 100;
            this.ContrastPicker.ValueChanged += (s, value) =>
            {
                float contrast = (float)value / 100.0f;
                this.Contrast = contrast;

                this.MethodViewModel.TAdjustmentChanged<float, ContrastAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Contrast = contrast,

                    type: HistoryType.LayersProperty_ResetAdjustment_Contrast_Contrast,
                    getUndo: (tAdjustment) => tAdjustment.Contrast,
                    setUndo: (tAdjustment, previous) => tAdjustment.Contrast = previous
                );
            };
        }

        private void ConstructContrast2()
        {
            this.ContrastSlider.SliderBrush = this.ContrastBrush;
            this.ContrastSlider.Minimum = -1.0d;
            this.ContrastSlider.Maximum = 1.0d;
            this.ContrastSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<ContrastAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheContrast());
            this.ContrastSlider.ValueChangeDelta += (s, value) =>
            {
                float contrast = (float)value;
                this.Contrast = contrast;

                this.MethodViewModel.TAdjustmentChangeDelta<ContrastAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.Contrast = contrast);
            };
            this.ContrastSlider.ValueChangeCompleted += (s, value) =>
            {
                float contrast = (float)value;
                this.Contrast = contrast;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, ContrastAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.Contrast = contrast,

                    type: HistoryType.LayersProperty_ResetAdjustment_Contrast_Contrast,
                    getUndo: (tAdjustment) => tAdjustment.StartingContrast,
                    setUndo: (tAdjustment, previous) => tAdjustment.Contrast = previous
                );
            };
        }

    }
}