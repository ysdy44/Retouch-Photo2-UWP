using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/>'s OuterShadowEffect .
    /// </summary>
    public class OuterShadowEffect : IEffect
    {
        OuterShadowPage OuterShadowPage { get; } = new OuterShadowPage();

        public EffectType Type => EffectType.OuterShadow;
        public Button Button { get; } = new Retouch_Photo2.Effects.Button(new OuterShadowControl());
        public Page Page => this.OuterShadowPage;


        public bool GetIsOn(EffectManager effectManager) => effectManager.OuterShadow_IsOn;
        public void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.OuterShadow_IsOn = isOn;
        public void Reset(EffectManager effectManager)
        {
            effectManager.OuterShadow_Radius = 0;
            effectManager.OuterShadow_Opacity = 0.5f;
            effectManager.OuterShadow_Color = Colors.Black;

            effectManager.OuterShadow_Offset = 0;
            effectManager.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.OuterShadowPage.RadiusSlider.Value = effectManager.OuterShadow_Radius;
            this.OuterShadowPage.OpacitySlider.Value = effectManager.OuterShadow_Opacity * 100.0f;
            this.OuterShadowPage.SolidColorBrush.Color = effectManager.OuterShadow_Color;

            this.OuterShadowPage.OffsetSlider.Value = effectManager.OuterShadow_Offset;
            this.OuterShadowPage.AnglePicker.Radians = effectManager.OuterShadow_Angle;
        }
    }
}