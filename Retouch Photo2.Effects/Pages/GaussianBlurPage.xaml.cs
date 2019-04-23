using Retouch_Photo2.Effects.Controls;
using Retouch_Photo2.Effects.Items;

namespace Retouch_Photo2.Effects.Pages
{
    public sealed partial class GaussianBlurPage : EffectPage
    {
        public GaussianBlurPage()
        {
            this.InitializeComponent();
            base.Type = EffectType.GaussianBlur;
            base.Control = new Control()
            {
                Icon = new GaussianBlurControl()
            };

            this.BlurAmountSlider.ValueChanged += (s, e) =>
            {
                  if (base.EffectManager == null) return;

                  base.EffectManager.GaussianBlurEffectItem.BlurAmount = (float)e.NewValue;
                  EffectManager.Invalidate?.Invoke();
            };
        }
        
        //@override
        public override bool GetIsOn(EffectManager manager) => manager.GaussianBlurEffectItem.IsOn;
        public override void SetIsOn(EffectManager manager, bool isOn) => manager.GaussianBlurEffectItem.IsOn = isOn;
                 
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
            this.BlurAmountSlider.Value = item.Amount;
        }
    }
}
