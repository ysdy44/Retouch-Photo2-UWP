using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "StraightenEffect" /> 's Page.
    /// </summary>
    public sealed partial class StraightenPage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public StraightenPage()
        {
            this.InitializeComponent();

            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Straighten_Angle = radians / 4.0f;
                });
            };
        }

        public void Reset()
        {
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Straighten_Angle = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.AnglePicker.Radians = effectManager.Straighten_Angle * 4.0f;
        }
    }
}