using Windows.ApplicationModel.DataTransfer;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Extensions used for layer drag and drop.
    /// </summary>
    public static class LayerDataPackageExpansion
    {
        /// <summary> The name of the format. </summary>
        public const string DataFormat = "Layer";

        /// <summary> Layer drag and drop. </summary>
        static Layer Layer;

        /// <summary>
        /// <see cref = "DataPackage" /> extending methods.
        /// Sets the layer that DataPackage contains
        /// </summary>
        /// <param name="layer"> The destination layer. </param>
        public static void SetLayer(this DataPackage dataPackage, Layer layer) => LayerDataPackageExpansion.Layer = layer;

        /// <summary>
        /// <see cref = "DataPackageView" /> extending methods.
        /// Gets the layer that DataPackage contains
        /// </summary>
        /// <returns> The destination layer. </returns>
        public static Layer GetLayer(this DataPackageView dataPackageView) => LayerDataPackageExpansion.Layer;

        /// <summary>
        /// Check that DataPackageView contains a specific data format.
        /// </summary>
        /// <param name="formatId"> The name of the format. </param>
        /// <returns> If DataPackageView contains this format, it is **true**, otherwise **false**. </returns>
        public static bool Contains2(this DataPackageView dataPackageView, string formatId) => (formatId == LayerDataPackageExpansion.DataFormat);
    }
}