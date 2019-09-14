using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using System;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentPage
    {

        public HueRotationAdjustment HueRotationAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        public FrameworkElement Page => this;
               
        //@Construct
        public HueRotationPage()
        {
            this.InitializeComponent();

            this.HueRotationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HueRotationAdjustment == null) return;
                this.HueRotationAdjustment.Angle = (float)(value * Math.PI / 180);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new HueRotationAdjustment();
        public void SetAdjustment(IAdjustment value)
        {
            if (value is HueRotationAdjustment adjustment)
            {
                this.HueRotationAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.HueRotationAdjustment = null;
        public void Reset()
        {
            if (this.HueRotationAdjustment == null) return;

            this.HueRotationAdjustment.Reset();
            this.Invalidate(this.HueRotationAdjustment);
        }

        public void Invalidate(HueRotationAdjustment adjustment)
        {
            this.HueRotationSlider.Value = adjustment.Angle * 180 / Math.PI;
        }
    }
}

