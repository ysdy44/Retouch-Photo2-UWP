using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Copy a layerage ( form Layerbase to Clipboard).
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        public static void CopyLayerage(Layerage layerage)
        {
            //
            ILayer layer = layerage.Self;
            ILayer clone2 = layer.Clone();
            Clipboard.Instances.Add(clone2);
            //

            LayerManager.CopyLayeragesCore(layerage.Children);
        }

        /// <summary>
        /// Copy layerages ( form Layerbase to Clipboard).
        /// </summary>
        /// <param name="layerages"> The layerages. </param>
        public static void CopyLayerages(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                LayerManager.CopyLayerage(layerage);
            }
        }


        private static void CopyLayeragesCore(IList<Layerage> children)
        {
            foreach (Layerage layerage in children)
            {
                LayerManager.CopyLayeragesCore(layerage.Children);

                //
                ILayer layer = layerage.Self;
                ILayer clone = layer.Clone();
                Clipboard.Instances.Add(clone);
                //
            }
        }



        /// <summary>
        /// Paste a layerage ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        public static Layerage PasteLayerage(Layerage layerage)
        {
            Layerage child = layerage.Clone();

            //
            ILayer childCopy = child.ClipboardSelf;
            ILayer cloneLayer = childCopy.Clone();

            Layerage cloneLayerage = Layerage.CreateByGuid();
            cloneLayer.Id = cloneLayerage.Id;
            LayerBase.Instances.Add(cloneLayerage.Id, cloneLayer);
            //

            cloneLayerage.Children = child.Children;
            child.Children = null;
            LayerManager.PasteLayerageCore(cloneLayerage.Children);
            return cloneLayerage;
        }

        /// <summary>
        /// Paste layerages ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="layerages"> The layerages. </param>
        public static IEnumerable<Layerage> PasteLayerages(IEnumerable<Layerage> layerages)
        {
            // When copying and pasting layers,
            // the order of the layers changes:

            // 1. Layers : ⬆ Positive order
            // 2. Clipboard : ⬇ Reverse order
            // 3. Paste : ⬆ Positive order         
            // 4. Reverse : ⬇ Reverse order
            // 5. Layers : ⬆ Positive order

            IEnumerable<Layerage> layerages2 = from l in layerages select LayerManager.PasteLayerage(l);
            return layerages2.Reverse();  // 4. Reverse
        }


        private static void PasteLayerageCore(IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerManager.PasteLayerageCore(child.Children);

                //
                ILayer childCopy = child.ClipboardSelf;
                ILayer cloneLayer = childCopy.Clone();

                Layerage cloneLayerage = Layerage.CreateByGuid();
                cloneLayer.Id = cloneLayerage.Id;
                LayerBase.Instances.Add(cloneLayerage.Id, cloneLayer);
                //

                children[i].Id = cloneLayerage.Id;
            }
        }

    }
}