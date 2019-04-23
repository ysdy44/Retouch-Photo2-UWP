using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Items;

namespace Retouch_Photo2.Effects.Pages
{
    public sealed partial class OuterShadowPage : EffectPage
    {
        public OuterShadowPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.OuterShadow;
            base.Control = new Control()
            {
                Icon = new OuterShadowControl()
            };

            this.RadiusSlider.ValueChanged += (s, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.OuterShadowEffectItem.Radius = (float)e.NewValue;
                EffectManager.Invalidate?.Invoke();
            };
            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.OuterShadowEffectItem.Opacity = (float)(e.NewValue / 100.0);
                EffectManager.Invalidate?.Invoke();
            };
            this.OffsetSlider.ValueChanged += (s, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.OuterShadowEffectItem.Offset = (float)e.NewValue;
                EffectManager.Invalidate?.Invoke();
            };
            this.AnglePicker.RadiansChange += (radians) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.OuterShadowEffectItem.Angle = radians;
                EffectManager.Invalidate?.Invoke();
            };
            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = base.EffectManager.OuterShadowEffectItem.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color = value;

                if (base.EffectManager == null) return;

                base.EffectManager.OuterShadowEffectItem.Color = value;
                EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.OuterShadowEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.OuterShadowEffectItem.IsOn = isOn;

        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.OuterShadowEffectItem);
        }
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            OuterShadowEffectItem item = base.EffectManager.OuterShadowEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(OuterShadowEffectItem item)
        {
            this.RadiusSlider.Value = item.Radius;
            this.OpacitySlider.Value = item.Opacity * 100.0;
            this.OffsetSlider.Value = item.Offset;
            this.AnglePicker.Radians = item.Angle;
            this.SolidColorBrush.Color = item.Color;
        }
    }
}
