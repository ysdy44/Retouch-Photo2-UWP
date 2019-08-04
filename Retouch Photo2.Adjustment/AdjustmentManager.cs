using Microsoft.Graphics.Canvas;
using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustment" />'s manager. 
    /// </summary>
    public class AdjustmentManager
    {
        //@Static
        public static Action Invalidate;

        public List<IAdjustment> Adjustments { get; set; } = new List<IAdjustment>();

        //@Static
        public static ICanvasImage Render(AdjustmentManager manager, ICanvasImage image)
        {
            if (manager.Adjustments.Count == 0) return image;
            if (manager.Adjustments.Count == 1) return manager.Adjustments.Single().GetRender(image);

            foreach (IAdjustment adjustment in manager.Adjustments)
            {
                image = adjustment.GetRender(image);
            }
            return image;
        }
    }
}