using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Layers
{
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