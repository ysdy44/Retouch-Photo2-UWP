using System.Collections.ObjectModel;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a cutboard used to paste layer(s).
    /// </summary>
    public partial class Clipboard
    {

        //@Static
        /// <summary> Collection <see cref="Layerage"/>s instances. </summary>
        public static ObservableCollection<ILayer> Instances = new ObservableCollection<ILayer>();


        /// <summary>
        /// Find the first <see cref="LayerBase"/> by <see cref="LayerBase"/>.
        /// </summary>
        /// <param name="layerage"> The source layerage</param>
        /// <returns> The product layer. </returns>
        public static ILayer FindFirstLayer(Layerage layerage)
        {
            string id = layerage.Id;
            return Clipboard.Instances.FirstOrDefault(i => i.Id == id);
        }
        
    }
}