using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class DirectionalBlurPage : EffectPage
    {     
        public DirectionalBlurPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.DirectionalBlur;
            base.Control = new Control()
            {
                Icon = new DirectionalBlurControl()
            };

            this.BlurAmountSlider.ValueChanged += (s, e) =>
            {
                  if (base.EffectManager == null) return;

                  base.EffectManager.DirectionalBlurEffectItem.BlurAmount = (float)e.NewValue;
                  EffectManager.Invalidate?.Invoke();
            };
            this.AnglePicker.RadiansChange += (radians) =>
            {
                  if (base.EffectManager == null) return;

                  base.EffectManager.DirectionalBlurEffectItem.Angle = radians;
                  EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.DirectionalBlurEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.DirectionalBlurEffectItem.IsOn= isOn;
                 
        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.DirectionalBlurEffectItem);
        }                 
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            DirectionalBlurEffectItem item = base.EffectManager.DirectionalBlurEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(DirectionalBlurEffectItem item)
        {
            this.BlurAmountSlider.Value = item.BlurAmount;
            this.AnglePicker.Radians = item.Angle;
        }
    }
}
