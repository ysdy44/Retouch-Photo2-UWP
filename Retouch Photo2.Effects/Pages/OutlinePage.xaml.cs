using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "OutlineEffect"/>.
    /// </summary>
    public sealed partial class OutlinePage : Page, IEffectPage
    {
        //@Content
        public FrameworkElement Self => this;

        //@Construct
        public OutlinePage()
        {
            this.InitializeComponent();

            this.SizeSlider.ValueChanged += (sender, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Outline_Size = (int)e.NewValue;
                });
            };
        }

        public void Reset()
        {
            this.SizeSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Outline_Size = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.SizeSlider.Value = effectManager.Outline_Size;
        }
    }
}