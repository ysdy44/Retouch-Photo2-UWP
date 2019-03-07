using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;

namespace Retouch_Photo.Adjustments.Models
{
    public class InvertAdjustment : Adjustment
    {
        public InvertAdjustmentItem InvertAdjustmentItem = new InvertAdjustmentItem();

        public InvertAdjustment()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            base.Item = this.InvertAdjustmentItem;
            base.HasPage = false;
            this.Reset();
        }

        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect { Source = image };
        }
    }
}
