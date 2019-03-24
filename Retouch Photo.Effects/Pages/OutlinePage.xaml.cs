using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
{
    public sealed partial class OutlinePage : EffectPage
    {
        public OutlinePage()
        {
            this.InitializeComponent();
            base.Type = EffectType.Outline;
            base.Control = new Control()
            {
                Icon = new OutlineControl()
            };

            this.SizeSlider.ValueChanged += (sender, e) =>
            {
                if (base.EffectManager == null) return;

                base.EffectManager.OutlineEffectItem.Size = (int)e.NewValue;
                EffectManager.Invalidate?.Invoke();
            };
        }

        //@override
        public override bool GetIsOn(EffectManager manager) => manager.OutlineEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.OutlineEffectItem.IsOn = isOn;

        public override void SetManager(EffectManager manager)
        {
            base.EffectManager = manager;
            this.Invalidate(base.EffectManager.OutlineEffectItem);
        }
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            OutlineEffectItem item = base.EffectManager.OutlineEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(OutlineEffectItem item)
        {
            this.SizeSlider.Value = item.Size;
        }
    }
}
