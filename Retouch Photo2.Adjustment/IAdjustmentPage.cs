using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustment"/>'s page.
    /// </summary>
    public interface IAdjustmentPage
    {
        /// <summary> Gets IAdjustmentPage's type. </summary>
        AdjustmentType Type { get; }
        /// <summary> Gets IAdjustmentPage's icon. </summary>
        FrameworkElement Icon { get; }
        /// <summary> Gets IAdjustmentPage's self. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets IAdjustmentPage's text. </summary>
        string Text { get; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        IAdjustment GetNewAdjustment();
    }
}