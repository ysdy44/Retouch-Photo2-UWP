using Microsoft.Graphics.Canvas;
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
        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        public static Action Invalidate;

        /// <summary>
        /// The source data.
        /// </summary>
        public List<IAdjustment> Adjustments { get; set; } = new List<IAdjustment>();

        //@Static
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="manager"> The adjustment-manager. </param>
        /// <param name="image"> The source image. </param>
        /// <returns> The rendered image. </returns>
        public static ICanvasImage GetRender(AdjustmentManager manager, ICanvasImage image)
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