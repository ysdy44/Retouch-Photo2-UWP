namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// <see cref = "IAdjustmentPage"/>'s page.
    /// </summary>
    public interface IAdjustmentGenericPage<T> : IAdjustmentPage
    {
        /// <summary> Gets IAdjustment's adjustment. </summary>
        T Adjustment { get; set; }

        /// <summary>
        /// Reset the <see cref="IAdjustmentPage"/>'s data.
        /// </summary>
        void Reset();
        /// <summary>
        /// <see cref="IAdjustmentPage"/>'s value follows the <see cref="IAdjustment"/>.
        /// </summary>
        /// <param name="adjustment"> The adjustment. </param>
        void Follow(T adjustment);
    }
}