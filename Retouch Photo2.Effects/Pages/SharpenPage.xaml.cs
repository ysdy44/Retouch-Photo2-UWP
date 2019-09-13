using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "SharpenEffect"/>.
    /// </summary>
    public sealed partial class SharpenPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public SharpenPage()
        {
            this.InitializeComponent();

            this.AmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Sharpen_Amount = (float)e.NewValue / 10.0f;
                });
            };
        }

        public void Reset()
        {
            this.AmountSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Sharpen_Amount = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.AmountSlider.Value = effectManager.Sharpen_Amount * 10.0f;
        }
    }
}