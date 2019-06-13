using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Pages;
using Windows.UI;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="Effect"/>'s OuterShadowEffect .
    /// </summary>
    public class OuterShadowEffect : Effect
    {
        //Icon
        readonly OuterShadowControl OuterShadowControl = new OuterShadowControl();
        //Page
        readonly OuterShadowPage OuterShadowPage = new OuterShadowPage();

        //@Construct
        public OuterShadowEffect()
        {
            base.Type = EffectType.OuterShadow;
            base.Button = new Retouch_Photo2.Effects.Button(this.OuterShadowControl);
            base.Page = this.OuterShadowPage;
        }

        //@override
        public override bool GetIsOn(EffectManager effectManager) => effectManager.OuterShadow_IsOn;
        public override void SetIsOn(EffectManager effectManager, bool isOn) => effectManager.OuterShadow_IsOn = isOn;
        public override void Reset(EffectManager effectManager)
        {
            effectManager.OuterShadow_Radius = 0;
            effectManager.OuterShadow_Opacity = 0.5f;
            effectManager.OuterShadow_Color = Colors.Black;

            effectManager.OuterShadow_Offset = 0;
            effectManager.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public override void SetPageValueByEffectManager(EffectManager effectManager)
        {
            this.OuterShadowPage.RadiusSlider.Value = effectManager.OuterShadow_Radius;
            this.OuterShadowPage.OpacitySlider.Value = effectManager.OuterShadow_Opacity * 100.0f;
            this.OuterShadowPage.SolidColorBrush.Color = effectManager.OuterShadow_Color;

            this.OuterShadowPage.OffsetSlider.Value = effectManager.OuterShadow_Offset;
            this.OuterShadowPage.AnglePicker.Radians = effectManager.OuterShadow_Angle;
        }
    }
}