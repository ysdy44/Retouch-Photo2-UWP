using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "EmbossEffect"/>.
    /// </summary>
    public sealed partial class EmbossPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public EmbossPage()
        {
            this.InitializeComponent();

            this.AmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                     effectManager.Emboss_Amount = (float)e.NewValue;
                });
            };
            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Emboss_Angle = radians;
                });
            };
        }

        public void Reset()
        {
            this.AmountSlider.Value = 0;
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Emboss_Amount = 0;
            effectManager.Emboss_Angle = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.AmountSlider.Value = effectManager.Emboss_Amount;
            this.AnglePicker.Radians = effectManager.Emboss_Angle;
        }
    }
}