using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustment"/>'s page.
    /// </summary>
    public interface IAdjustmentPage
    {
        /// <summary> IAdjustmentPage's type. </summary>
        AdjustmentType Type { get; }
        /// <summary> IAdjustmentPage's icon. </summary>
        FrameworkElement Icon { get; }
        /// <summary> IAdjustmentPage's Page. </summary>
        FrameworkElement Page { get; }
        
        /// <summary> Return a new <see cref = "IAdjustment"/>. </summary>
        IAdjustment GetNewAdjustment();
        /// <summary> Return the current <see cref = "IAdjustment"/>. </summary>
        IAdjustment GetAdjustment();
        /// <summary> Assignment the current <see cref = "IAdjustment"/>. </summary>
        void SetAdjustment(IAdjustment adjustment);

        /// <summary> Call this method, when the IAdjustmentPage navigated. </summary>
        void Close();
        /// <summary> Make <see cref = "IAdjustment"/> and IAdjustmentPage back to initial state. </summary>
        void Reset();
    }
}