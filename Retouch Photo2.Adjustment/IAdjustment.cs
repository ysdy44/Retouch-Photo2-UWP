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
        /// <summary> Gets whether page is visible. </summary>
        Visibility PageVisibility { get; }
        /// <summary> Gets IAdjustment's page. </summary>
        UIElement Page { get; }
        /// <summary> Gets IAdjustment's text. </summary>
        string Text { get; }

        /// <summary> Reset the adjustment. </summary>
        void Reset();
        /// <summary> Follow the adjustment for page. </summary>
        void Follow();
        /// <summary> Close the adjustment's page. </summary>
        void Close();

        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned <see cref="IAdjustment"/>. </returns>
        IAdjustment Clone();
        
        /// <summary>
        /// Saves the entire <see cref="IAdjustment"/> to a XElement.
        /// </summary>
        /// <returns> The destination XElement. </returns>
        void SaveWith(XElement element);
        /// <summary>
        /// Load the entire <see cref="IAdjustment"/> by a XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        void Load(XElement element);
       
        /// <summary>
        /// Gets a specific rended-image.
        /// </summary>
        /// <param name="image"> previousImage </param>
        /// <returns> The rendered adjustment. </returns>
        ICanvasImage GetRender(ICanvasImage image);
    }
}