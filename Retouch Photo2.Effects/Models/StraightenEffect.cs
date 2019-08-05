using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s StraightenEffect .
    /// </summary>
    public class StraightenEffect : IEffect
    {
        StraightenPage StraightenPage { get; } = new StraightenPage();

        public EffectType Type => EffectType.Straighten;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new StraightenControl());
        public Page Page => this.StraightenPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.Straighten_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Straighten_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.Straighten_Angle = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.StraightenPage.AnglePicker.Radians = effectManager.Straighten_Angle * 4.0f; ;
        }
    }
}