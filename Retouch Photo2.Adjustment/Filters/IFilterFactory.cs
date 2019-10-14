using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Factory to provide filters and adjustments.
    /// </summary>
    public interface IFilterFactory
    {

        /// <summary>
        /// [Json] --> List [Filter]
        /// </summary>
        /// <param name="json"> The source json. </param>
        /// <returns> The provided filters. </returns>
        IEnumerable<Filter> CreateFilters(string json);

        /// <summary>
        /// [Json] --> [Filter]
        /// </summary>
        /// <param name="json"> The source json. </param>
        /// <returns> The provided filter. </returns>
        Filter CreateFilter(string json);

        /// <summary>
        /// [Json] --> [Adjustment]
        /// </summary>
        /// <param name="json"> The source json. </param>
        /// <returns> The provided filter. </returns>
        IAdjustment CreateAdjustment(string json);
    }
}