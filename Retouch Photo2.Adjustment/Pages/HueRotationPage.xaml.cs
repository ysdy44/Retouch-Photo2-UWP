using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HueRotationAdjustment"/>.
    /// </summary>
    public sealed partial class HueRotationPage : IAdjustmentPage
    {
        public HueRotationAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.HueRotation;
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public HueRotationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.HueRotationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Angle = (float)(value * Math.PI / 180);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new HueRotationAdjustment();

        public void Follow(HueRotationAdjustment adjustment)
        {
            this.HueRotationSlider.Value = adjustment.Angle * 180 / Math.PI;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HueRotation");

            this.AngleTextBlock.Text = resource.GetString("/Adjustments/HueRotation_Angle");
        }

    }
}

