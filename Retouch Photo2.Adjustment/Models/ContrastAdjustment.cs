using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s ContrastAdjustment.
    /// </summary>
    public class ContrastAdjustment : Adjustment
    {
        public const string Name = "Contrast";
        public ContrastAdjustmentItem ContrastAdjustmentItem=new ContrastAdjustmentItem();

        //@Construct
        public ContrastAdjustment()
        {
            base.Type = AdjustmentType.Contrast;
            base.Icon = new ContrastControl();
            base.Item = this.ContrastAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}