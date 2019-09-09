using Microsoft.Graphics.Canvas;
using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Provides icon and data for adjustments.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public interface IAdjustment
    {
        /// <summary> Gets adjustment's type. </summary>
        [JsonProperty]
        string TypeName { get; }

        /// <summary> Gets IAdjustment's type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets IAdjustment's icon. </summary>
        FrameworkElement Icon { get; }
        /// <summary> Gets whether page is visible. </summary>
        Visibility Visibility { get; }
        
        /// <summary> Reset the adjustment data. </summary>
        void Reset();

        /// <summary>
        /// Gets a specific rended-image.
        /// </summary>
        /// <param name="image"> previousImage </param>
        /// <returns> The rendered adjustment. </returns>
        ICanvasImage GetRender(ICanvasImage image);
        /// <summary>
        /// Get IAdjustment own copy.
        /// </summary>
        /// <returns> The cloned IAdjustment. </returns>
        IAdjustment Clone();
    }
    
    /// <summary> <see cref = "IAdjustment" />'s substitute. </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Adjustment2
    {
        /// <summary> Gets or Stes adjustment's type. </summary>
        [JsonProperty]
        public string TypeName { get; }
    }
}