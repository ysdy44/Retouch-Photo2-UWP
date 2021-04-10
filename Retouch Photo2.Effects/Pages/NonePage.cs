// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★★★
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    public class NonePage : IEffectPage
    {
        //@Content
        public EffectType Type => EffectType.None;
        public ControlTemplate Icon { get; set; }
        public FrameworkElement Self => null;


        public bool FollowButton(Effect effect) => false;
        public void FollowPage(Effect effect) { }
        public void Reset() { }
        public void Switch(bool isOn) { }
    }
}
