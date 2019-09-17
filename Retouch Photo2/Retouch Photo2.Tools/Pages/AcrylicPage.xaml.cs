using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary> 
    /// Page of <see cref = "AcrylicTool"/>.
    /// </summary>
    public sealed partial class AcrylicPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;


        //@Converter
        private int TintOpacityNumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private int BlurAmountNumberConverter(float blurAmount) => (int)blurAmount;

        private bool AcrylicTintOpacityTypeConverter(TouchbarType type) => type == TouchbarType.AcrylicTintOpacity;
        private bool AcrylicBlurAmountTypeConverter(TouchbarType type) => type == TouchbarType.AcrylicBlurAmount;

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }
        

        //@Construct
        public AcrylicPage()
        {
            this.InitializeComponent();

            //TintOpacity
            this.TintOpacityTouchbarButton.Unit = "%";
            this.TintOpacityTouchbarButton.Toggled += (s, isChecked) =>
            {
                if (isChecked)
                    this.TipViewModel.SetTouchbar(TouchbarType.None);//Touchbar
                else
                    this.TipViewModel.SetTouchbar(TouchbarType.AcrylicTintOpacity);//Touchbar
            };

            //BlurAmount
            this.BlurAmountTouchbarButton.Unit = "dp";
            this.BlurAmountTouchbarButton.Toggled += (s, isChecked) =>
            {
                if (isChecked)
                    this.TipViewModel.SetTouchbar(TouchbarType.None);//Touchbar
                else
                    this.TipViewModel.SetTouchbar(TouchbarType.AcrylicBlurAmount);//Touchbar
            };
        }
    }
}