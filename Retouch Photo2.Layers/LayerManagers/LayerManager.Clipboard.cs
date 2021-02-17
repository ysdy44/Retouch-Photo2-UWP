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

            LayerManager._copyLayerage(layerage.Children);
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


        private static void _copyLayerage(IList<Layerage> children)
        {
            foreach (Layerage layerage in children)
            {
                LayerManager._copyLayerage(layerage.Children);

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
            ILayer child2 = child.ClipboardSelf;
            ILayer clone2 = child2.Clone();
            Layerage clone = clone2.ToLayerage();
            LayerBase.Instances.Add(clone2);
            //

            clone.Children = child.Children;
            child.Children = null;
            LayerManager._pasteLayerage(clone.Children);
            return clone;
        }

        /// <summary>
        /// Paste layerages ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="layerages"> The layerages. </param>
        public static IEnumerable<Layerage> PasteLayerages(IEnumerable<Layerage> layerages)
        {
            return from l in layerages select LayerManager.PasteLayerage(l);
        }


        private static void _pasteLayerage(IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerManager._pasteLayerage(child.Children);

                //
                ILayer child2 = child.ClipboardSelf;
                ILayer clone2 = child2.Clone();
                Layerage clone = clone2.ToLayerage();
                LayerBase.Instances.Add(clone2);
                //

                children[i].Id = clone.Id;
            }
        }

    }
}