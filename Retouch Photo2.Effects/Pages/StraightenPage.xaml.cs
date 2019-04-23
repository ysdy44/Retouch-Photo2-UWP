using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Items;

namespace Retouch_Photo2.Effects.Pages
{
    public sealed partial class StraightenPage : EffectPage
    {
        public StraightenPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.Straighten;
            base.Control = new Control()
            {
                Icon = new StraightenControl()
            };

            this.AnglePicker.RadiansChange += (radians) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.StraightenEffectItem.Angle = radians / 4.0f;
                EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.StraightenEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.StraightenEffectItem.IsOn = isOn;

        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.StraightenEffectItem);
        }
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            StraightenEffectItem item = base.EffectManager.StraightenEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(StraightenEffectItem item)
        {
            this.AnglePicker.Radians = item.Angle * 4.0f;
        }
    }
}
