using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class EmbossPage : EffectPage
    {
        public EmbossPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.Emboss;
            base.Control = new Control()
            {
                Icon = new EmbossControl()
            };

            this.AmountSlider.ValueChanged += (s, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.EmbossEffectItem.Amount = (float)e.NewValue;
                EffectManager.Invalidate?.Invoke();
            };
            this.AnglePicker.RadiansChange += (radians) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.EmbossEffectItem.Angle = radians;
                EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.EmbossEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.EmbossEffectItem.IsOn = isOn;

        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.EmbossEffectItem);
        }
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            base.EffectManager.StraightenEffectItem.Reset();
            EmbossEffectItem item = base.EffectManager.EmbossEffectItem;
            this.Invalidate(item);
        }
        public void Invalidate(EmbossEffectItem item)
        {
            this.AmountSlider.Value = item.Amount;
            this.AnglePicker.Radians = item.Angle;
        }
    }
}
