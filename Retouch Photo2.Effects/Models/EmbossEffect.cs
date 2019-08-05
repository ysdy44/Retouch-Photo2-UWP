using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s EmbossEffect .
    /// </summary>
    public class EmbossEffect : IEffect
    {
        EmbossPage EmbossPage { get; } = new EmbossPage();

        public EffectType Type => EffectType.Emboss;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new EmbossControl());
        public Page Page => this.EmbossPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.Emboss_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Emboss_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.Emboss_Amount = 0;
            effectManager.Emboss_Angle = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.EmbossPage.AmountSlider.Value = effectManager.Emboss_Amount;
            this.EmbossPage.AnglePicker.Radians = effectManager.Emboss_Angle;
        }
    }
}