using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "GaussianBlurEffect" /> 's Page.
    /// </summary>
    public sealed partial class GaussianBlurPage : Page
    {
        /// <summary> <see cref = "GaussianBlurPage" />'s BlurAmountSlider. </summary>
        public Slider BlurAmountSlider => this._BlurAmountSlider;

        //@Construct
        public GaussianBlurPage()
        {
            this.InitializeComponent();

            this._BlurAmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.GaussianBlur_BlurAmount = (float)e.NewValue;
                });
            };
        }        
    }
}