// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★★★
using Microsoft.Graphics.Canvas;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Represents a special adjustment that adds adjustments to layers.
    /// </summary>
    public interface IAdjustment
    {
        /// <summary> Gets the type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets whether page is visible. </summary>
        Visibility PageVisibility { get; }

        ICommand Edit { get; }
        ICommand Remove { get; }

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
        /// Gets a specific rended-adjustment.
        /// </summary>
        /// <param name="image"> previousImage </param>
        /// <returns> The rendered adjustment. </returns>
        ICanvasImage GetRender(ICanvasImage image);
    }
}