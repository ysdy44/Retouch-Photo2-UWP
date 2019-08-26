using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// <see cref = "StraightenEffect" /> 's Page.
    /// </summary>
    public sealed partial class StraightenPage : Page
    {
        /// <summary> <see cref = "StraightenPage" />'s AnglePicker. </summary>
        public RadiansPicker AnglePicker => this._AnglePicker;

        //@Construct
        public StraightenPage()
        {
            this.InitializeComponent();

            this._AnglePicker.RadiansChange += (s, radians) =>
            {
                EffectManager.Invalidate((effectManager) =>
                {
                    effectManager.Straighten_Angle = radians / 4.0f;
                });
            };
        }        
    }
}