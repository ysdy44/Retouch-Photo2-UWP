using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo.Adjustments
{
    /// <summary> <see cref = "Adjustment" />'s manager. </summary>
    public class AdjustmentManager
    {
        public List<Adjustment> Adjustments { get; set; } = new List<Adjustment>();

        //@static
        public static ICanvasImage Render(AdjustmentManager manager, ICanvasImage image)
        {
            if (manager.Adjustments == null) return image;
            if (manager.Adjustments.Count == 0) return image;
            if (manager.Adjustments.Count == 1) return manager.Adjustments.Single().GetRender(image);

            foreach (Adjustment item in manager.Adjustments)
            {
                image = item.GetRender(image);
            }
            return image;
        }
    }
}