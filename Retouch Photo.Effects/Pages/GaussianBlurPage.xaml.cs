using Retouch_Photo.Effects.Controls;
using Retouch_Photo.Effects.Items;

namespace Retouch_Photo.Effects.Pages
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
            this.Invalidate(base.EffectManager.GaussianBlurEffectItem);
        }        
        public override void Reset()
        {
            if (base.EffectManager == null) return;

            GaussianBlurEffectItem item = base.EffectManager.GaussianBlurEffectItem;
            item.Reset();
            this.Invalidate(item);
        }
        public void Invalidate(GaussianBlurEffectItem item)
        {
            this.BlurAmountSlider.Value = item.BlurAmount;
        }
    }
}
