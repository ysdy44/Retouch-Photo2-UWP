using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "EmbossEffect"/>.
    /// </summary>
    public sealed partial class EmbossPage : Page
    {
        /// <summary> <see cref = "EmbossPage" />'s BlurAmountSlider. </summary>
        public Slider AmountSlider => this._AmountSlider;
        /// <summary> <see cref = "EmbossPage" />'s RadiansPicker. </summary>
        public RadiansPicker AnglePicker => this._AnglePicker;

        //@Construct
        public EmbossPage()
        {
            this.InitializeComponent();

            this._AmountSlider.ValueChanged += (s, e) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                     effectManager.Emboss_Amount = (float)e.NewValue;
                });
            };
            this._AnglePicker.RadiansChange += (radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Emboss_Angle = radians;
                });
            };
        }        
    }
}