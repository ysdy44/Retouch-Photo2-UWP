using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Filters
{
    /// <summary> 
    /// <see cref = "IAdjustment" />'s. 
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The source data.
        /// </summary>
        public List<IAdjustment> Adjustments { get; set; } = new List<IAdjustment>();

        //@Static
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="filter"> The filter. </param>
        /// <param name="image"> The source image. </param>
        /// <returns> The rendered image. </returns>
        public static ICanvasImage Render(Filter filter, ICanvasImage image)
        {
            if (filter.Adjustments.Count == 0) return image;
            if (filter.Adjustments.Count == 1) return filter.Adjustments.Single().GetRender(image);

            foreach (IAdjustment adjustment in filter.Adjustments)
            {
                image = adjustment.GetRender(image);
            }
            return image;
        }

    }
}