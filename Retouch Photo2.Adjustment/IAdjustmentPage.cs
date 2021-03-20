// Core:              ★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustment"/>'s page.
    /// </summary>
    public interface IAdjustmentPage
    {
        /// <summary> Gets the type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets the icon. </summary>
        ControlTemplate Icon { get; set; }
        /// <summary> Gets the self. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets the text. </summary>
        string Text { get; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        IAdjustment GetNewAdjustment();


        /// <summary> Gets the adjustment index. </summary>
        int Index { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        void Reset();
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        void Follow();
    }
}