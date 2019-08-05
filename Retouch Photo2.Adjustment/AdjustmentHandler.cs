using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    //@Delegate
    /// <summary>
    /// Represents the method of handling events
    /// </summary>
    /// <param name="adjustment"> Adjustment data for event processing. </param>
    public delegate void AdjustmentHandler(IAdjustment adjustment);

    /// <summary>
    /// Represents the method of handling events
    /// </summary>
    /// <param name="adjustments"> Adjustments data for event processing. </param>
    public delegate void AdjustmentsHandler(IEnumerable<IAdjustment> adjustments);
}