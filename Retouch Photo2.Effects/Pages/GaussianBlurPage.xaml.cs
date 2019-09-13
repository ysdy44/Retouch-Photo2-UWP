using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "GaussianBlurEffect"/>.
    /// </summary>
    public sealed partial class GaussianBlurPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public GaussianBlurPage()
        {
            this.InitializeComponent();

            this.BlurAmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.GaussianBlur_BlurAmount = (float)e.NewValue;
                });
            };
        }

        public void Reset()
        {
            this.BlurAmountSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.GaussianBlur_BlurAmount = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.BlurAmountSlider.Value = effectManager.GaussianBlur_BlurAmount;
        }
    }
}