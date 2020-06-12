using Windows.UI.Xaml;

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
        FrameworkElement Icon { get; }
        /// <summary> Gets the self. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets the text. </summary>
        string Text { get; }

        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        IAdjustment GetNewAdjustment();
    }
}