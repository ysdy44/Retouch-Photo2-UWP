using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "DirectionalBlurEffect"/>.
    /// </summary>
    public sealed partial class DirectionalBlurPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public DirectionalBlurPage()
        {
            this.InitializeComponent();

            this.BlurAmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.DirectionalBlur_BlurAmount = (float)e.NewValue;
                });
            };
            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.DirectionalBlur_Angle = radians;
                });
            };
        }


        public void Reset()
        {
            this.BlurAmountSlider.Value = 0;
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.DirectionalBlur_BlurAmount = 0;
            effectManager.DirectionalBlur_Angle = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.BlurAmountSlider.Value = effectManager.DirectionalBlur_BlurAmount;
            this.AnglePicker.Radians = effectManager.DirectionalBlur_Angle;
        }
    }
}