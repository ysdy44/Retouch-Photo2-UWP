using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class HueRotationAdjustment : Adjustment
    {
        public float Angle;

        public HueRotationAdjustment()
        {
            base.Type = AdjustmentType.HueRotation;
            base.Icon = new AdjustmentHueRotationControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.Angle = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new HueRotationEffect
            {
                Angle = this.Angle,
                Source = image
            };
        }
    }


    public class HueRotationAdjustmentCandidate : AdjustmentCandidate
    {
        public AdjustmentHueRotationPage page = new AdjustmentHueRotationPage();

        public HueRotationAdjustmentCandidate()
        {
            base.Type = AdjustmentType.HueRotation;
            base.Icon = new AdjustmentHueRotationControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new HueRotationAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is HueRotationAdjustment HueRotationAdjustment)
            {
                this.page.HueRotationAdjustment = null;
                this.page.HueRotationAdjustment = HueRotationAdjustment;
            }
        }
    }
}

