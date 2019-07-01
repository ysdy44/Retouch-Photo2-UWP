using Retouch_Photo2.Effects.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "OutlineEffect"/>.
    /// </summary>
    public sealed partial class OutlinePage : Page
    {
        /// <summary> <see cref = "OuterShadowPage" />'s SizeSlider. </summary>
        public Slider SizeSlider => this._SizeSlider;

        //@Construct
        public OutlinePage()
        {
            this.InitializeComponent();

            this._SizeSlider.ValueChanged += (sender, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Outline_Size = (int)e.NewValue;
                });
            };
        }        
    }
}