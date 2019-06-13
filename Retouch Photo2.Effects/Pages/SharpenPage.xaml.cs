using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "SharpenEffect" /> 's Page.
    /// </summary>
    public sealed partial class SharpenPage : Page
    {
        /// <summary> <see cref = "SharpenPage" />'s AmountSlider. </summary>
        public Slider AmountSlider => this._AmountSlider;

        //@Construct
        public SharpenPage()
        {
            this.InitializeComponent();

            this._AmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Sharpen_Amount = (float)e.NewValue / 10.0f;
                });
            };
        }        
    }
}