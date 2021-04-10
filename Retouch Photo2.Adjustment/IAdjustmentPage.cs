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

        /// <summary> Gets the title. </summary>
        string Title { get; }
        /// <summary> Gets the icon. </summary>
        ControlTemplate Icon { get; }
        /// <summary> Gets the self. </summary>
        FrameworkElement Self { get; }


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