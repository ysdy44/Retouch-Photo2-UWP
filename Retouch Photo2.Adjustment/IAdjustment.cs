// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★★★
using Microsoft.Graphics.Canvas;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Provides icon and data for adjustments.
    /// </summary>
    public interface IAdjustment
    {
        /// <summary> Gets the type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets whether page is visible. </summary>
        Visibility PageVisibility { get; }
        /// <summary> Gets the page. </summary>
        IAdjustmentPage Page { get; }
        /// <summary> Gets the icon. </summary>
        ControlTemplate Icon { get; }
        /// <summary> Gets the text. </summary>
        string Text { get; }

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
        /// Gets a specific rended-image.
        /// </summary>
        /// <param name="image"> previousImage </param>
        /// <returns> The rendered adjustment. </returns>
        ICanvasImage GetRender(ICanvasImage image);
    }
}