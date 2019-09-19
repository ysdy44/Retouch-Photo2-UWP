using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
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

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }
        

        //@Construct
        public AcrylicPage()
        {
            this.InitializeComponent();

            //TintOpacity
            this.TintOpacityTouchbarButton.Unit = "%";

            //BlurAmount
            this.BlurAmountTouchbarButton.Unit = "dp";

            //More
            this.MoreButton.Tapped += (s, e) =>
            {
                this.TipViewModel.TouchbarType = TouchbarType.None;//Touchbar
                this.Flyout.ShowAt(this);
            };
        }
    }
}