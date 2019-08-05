using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s OutlineEffect .
    /// </summary>
    public class OutlineEffect : IEffect
    {
        OutlinePage OutlinePage { get; } = new OutlinePage();

        public EffectType Type => EffectType.Outline;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new OutlineControl());
        public Page Page => this.OutlinePage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.Outline_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.Outline_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.Outline_Size = 0;
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.OutlinePage.SizeSlider.Value = effectManager.Outline_Size;
        }
    }
}