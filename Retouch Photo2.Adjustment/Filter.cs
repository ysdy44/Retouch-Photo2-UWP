using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustment" />'s. 
    /// </summary>
    public class Filter
    {
        //@Static
        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        public static Action Invalidate;

        public string Name { get; set; } = string.Empty;

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
        public static ICanvasImage GetRender(Filter manager, ICanvasImage image)
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