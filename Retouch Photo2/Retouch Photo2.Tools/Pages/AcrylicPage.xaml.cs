using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
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

        //@Touchbar
        AcrylicTintOpacityTouchbarSlider _tintOpacityTouchbarSlider { get; } = new AcrylicTintOpacityTouchbarSlider();
        AcrylicBlurAmountTouchbarSlider _blurAmountTouchbarSlider { get; } = new AcrylicBlurAmountTouchbarSlider();

        //@Converter
        private int TintOpacityNumberConverter(float tintOpacity) => (int)(tintOpacity * 100d);
        private int BlurAmountNumberConverter(float blurAmount) => (int)blurAmount;

        /// <summary> Type of AcrylicPage. </summary>
        public AcrylicToolType Type
        {
            set
            {
                switch (value)
                {
                    case AcrylicToolType.None:
                        {
                            this.TintOpacityTouchbarButton.IsChecked = false;
                            this.BlurAmountTouchbarButton.IsChecked = false;
                            this.TipViewModel.Touchbar = null;//Touchbar
                        }
                        break;
                    case AcrylicToolType.TintOpacity:
                        {
                            this.TintOpacityTouchbarButton.IsChecked = true;
                            this.BlurAmountTouchbarButton.IsChecked = false;
                            this.TipViewModel.Touchbar = this._tintOpacityTouchbarSlider;//Touchbar
                        }
                        break;
                    case AcrylicToolType.BlurAmount:
                        {
                            this.TintOpacityTouchbarButton.IsChecked = false;
                            this.BlurAmountTouchbarButton.IsChecked = true;
                            this.TipViewModel.Touchbar = this._blurAmountTouchbarSlider;//Touchbar
                        }
                        break;
                }
            }
        }

        //@Construct
        public AcrylicPage()
        {
            this.InitializeComponent();

            //TintOpacity
            this.TintOpacityTouchbarButton.Unit = "%";
            this.TintOpacityTouchbarButton.Tapped2 += (s, isChecked) =>
            {
                if (isChecked) this.Type = AcrylicToolType.None;
                else this.Type = AcrylicToolType.TintOpacity;
            };

            //BlurAmount
            this.BlurAmountTouchbarButton.Unit = "dp";
            this.BlurAmountTouchbarButton.Tapped2 += (s, isChecked) =>
            {
                if (isChecked) this.Type = AcrylicToolType.None;
                else this.Type = AcrylicToolType.BlurAmount;
            };
        }
    }
}