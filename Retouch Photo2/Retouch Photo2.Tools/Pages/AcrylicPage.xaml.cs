using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    internal enum AcrylicMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Tint-opacity. </summary>
        TintOpacity,

        /// <summary> Tint-amount. </summary>
        BlurAmount,
    }

    /// <summary> 
    /// Page of <see cref = "AcrylicTool"/>.
    /// </summary>
    public sealed partial class AcrylicPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        
        //@TouchBar
        internal AcrylicMode TouchBarMode
        {
            set
            {
                switch (value)
                {
                    case AcrylicMode.None:
                        this.TintOpacityTouchbarButton.IsSelected = false;
                        this.BlurAmountTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case AcrylicMode.TintOpacity:
                        this.TintOpacityTouchbarButton.IsSelected = true;
                        this.BlurAmountTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.TintOpacityTouchbarSlider;
                        break;
                    case AcrylicMode.BlurAmount:
                        this.TintOpacityTouchbarButton.IsSelected = false;
                        this.BlurAmountTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.BlurAmountTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int BlurAmountNumberConverter(float blurAmount) => (int)blurAmount;
        private double BlurAmountValueConverter(float blurAmount) => blurAmount;
        
        private int TintOpacityNumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private double TintOpacityValueConverter(float tintOpacity) => tintOpacity * 100d;

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;


        //@Construct
        public AcrylicPage()
        {
            this.InitializeComponent();

            //BlurAmount
            {
                //Button
                this.BlurAmountTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this.TouchBarMode = AcrylicMode.BlurAmount;
                    else
                        this.TouchBarMode = AcrylicMode.None;
                };

                //Number
                this.BlurAmountTouchbarSlider.Unit = "dp";
                this.BlurAmountTouchbarSlider.NumberMinimum = 10;
                this.BlurAmountTouchbarSlider.NumberMaximum = 100;
                this.BlurAmountTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float amount = number;
                    this.BlurAmountChange(amount);
                };

                //Value
                this.BlurAmountTouchbarSlider.Minimum = 10d;
                this.BlurAmountTouchbarSlider.Maximum = 100d;
                this.BlurAmountTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.BlurAmountTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float amount = (float)value;
                    this.BlurAmountChange(amount);
                };
                this.BlurAmountTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            //TintOpacity
            {
                //Button
                this.TintOpacityTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this.TouchBarMode = AcrylicMode.TintOpacity;
                    else
                        this.TouchBarMode = AcrylicMode.None;
                };

                //Number
                this.TintOpacityTouchbarSlider.Unit = "%";
                this.TintOpacityTouchbarSlider.NumberMinimum = 0;
                this.TintOpacityTouchbarSlider.NumberMaximum = 90;
                this.TintOpacityTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float tintOpacity = number / 100f;
                    this.TintOpacityChange(tintOpacity);
                };

                //Value
                this.TintOpacityTouchbarSlider.Minimum = 0d;
                this.TintOpacityTouchbarSlider.Maximum = 90d;
                this.TintOpacityTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.TintOpacityTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float tintOpacity = (float)(value / 100d);
                    this.TintOpacityChange(tintOpacity);
                };
                this.TintOpacityTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

        }

        private void BlurAmountChange(float amount)
        {
            if (amount < 10.0f) amount = 10.0f;
            if (amount > 100.0f) amount = 100.0f;

            this.SelectionViewModel.AcrylicBlurAmount = amount;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.BlurAmount = amount;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void TintOpacityChange(float opacity)
        {
            this.SelectionViewModel.AcrylicTintOpacity = opacity;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is AcrylicLayer acrylicLayer)
                {
                    acrylicLayer.TintOpacity = opacity;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = AcrylicMode.None;
        }
    }
}