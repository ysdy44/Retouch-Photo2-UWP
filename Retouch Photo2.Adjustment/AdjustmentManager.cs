using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "Adjustment" />'s manager. 
    /// </summary>
    public class AdjustmentManager
    {
        public List<Adjustment> Adjustments { get; set; } = new List<Adjustment>();

        //@Static
        public static ICanvasImage Render(AdjustmentManager manager, ICanvasImage image)
        {
            if (manager.Adjustments.Count == 0) return image;
            if (manager.Adjustments.Count == 1) return manager.Adjustments.Single().Item.GetRender(image);

            foreach (var item in manager.Adjustments)
            {
                image = item.Item.GetRender(image);
            }
            return image;
        }
    }
}