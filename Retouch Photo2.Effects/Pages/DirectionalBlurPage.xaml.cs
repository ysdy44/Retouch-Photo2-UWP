using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "DirectionalBlurEffect" /> 's Page.
    /// </summary>
    public sealed partial class DirectionalBlurPage : Page
    {
        /// <summary> <see cref = "DirectionalBlurPage" />'s BlurAmountSlider. </summary>
        public Slider BlurAmountSlider => this._BlurAmountSlider;
        /// <summary> <see cref = "DirectionalBlurPage" />'s RadiansPicker. </summary>
        public RadiansPicker AnglePicker => this._AnglePicker;

        //@Construct
        public DirectionalBlurPage()
        {
            this.InitializeComponent();

            this._BlurAmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.DirectionalBlur_BlurAmount = (float)e.NewValue;
                });
            };
            this._AnglePicker.RadiansChange += (radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.DirectionalBlur_Angle = radians;
                });
            };
        }        
    }
}