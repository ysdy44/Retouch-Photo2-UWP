using Microsoft.Graphics.Canvas;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Provides icon and data for adjustments.
    /// </summary>
    public interface IAdjustment
    {
        /// <summary> Gets IAdjustment's type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets IAdjustment's icon. </summary>
        FrameworkElement Icon { get; }
        /// <summary> Gets whether page is visible. </summary>
        Visibility PageVisibility { get; }
        
        /// <summary> Reset the adjustment data. </summary>
        void Reset();
        /// <summary>
        /// Get IAdjustment own copy.
        /// </summary>
        /// <returns> The cloned IAdjustment. </returns>
        IAdjustment Clone();
        /// <summary>
        /// Saves the entire IAdjustment to a XElement.
        /// </summary>
        /// <returns> The saved IAdjustment. </returns>
        XElement Save();

        /// <summary>
        /// Gets a specific rended-image.
        /// </summary>
        /// <param name="image"> previousImage </param>
        /// <returns> The rendered adjustment. </returns>
        ICanvasImage GetRender(ICanvasImage image);
    }
}