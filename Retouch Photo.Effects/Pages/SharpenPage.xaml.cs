using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class SharpenPage : EffectPage
    {
        public SharpenPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.Sharpen;
            base.Control = new Control()
            {
                Icon = new SharpenControl()
            };

            this.AmountSlider.ValueChanged += (s, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.SharpenEffectItem.Amount = (float)e.NewValue / 10.0f;
                EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.SharpenEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.SharpenEffectItem.IsOn = isOn;

        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.SharpenEffectItem);
        }
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            SharpenEffectItem item = base.EffectManager.SharpenEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(SharpenEffectItem item)
        {
            this.AmountSlider.Value = item.Amount*10.0f;
        }
    }
}

