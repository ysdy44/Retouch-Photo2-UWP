using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerageCollection
    {
        
        /// <summary>
        /// Copy a layerage ( form Layerbase to Clipboard).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param> 
        /// <param name="layerage"> The layerage. </param>
        public static void CopyLayerage(CanvasDevice customDevice, Layerage layerage)
        {
            //
            ILayer layer = layerage.Self;
            ILayer clone2 = layer.Clone(customDevice);
            Clipboard.Instances.Add(clone2);
            //

            LayerageCollection._copyLayerage(customDevice, layerage.Children);
        }

        /// <summary>
        /// Copy layerages ( form Layerbase to Clipboard).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerages"> The layerages. </param>
        public static void CopyLayerages(CanvasDevice customDevice, IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                LayerageCollection.CopyLayerage(customDevice, layerage);
            }
        }


        private static void _copyLayerage(CanvasDevice customDevice, IList<Layerage> children)
        {
            foreach (Layerage layerage in children)
            {
                LayerageCollection._copyLayerage(customDevice, layerage.Children);

                //
                ILayer layer = layerage.Self;
                ILayer clone = layer.Clone(customDevice);
                Clipboard.Instances.Add(clone);
                //
            }
        }

       

        /// <summary>
        /// Paste a layerage ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerage"> The layerage. </param>
        public static Layerage PasteLayerage(CanvasDevice customDevice, Layerage layerage)
        {
            Layerage child = layerage.Clone();

            //
            ILayer child2 = child.ClipboardSelf;
            ILayer clone2 = child2.Clone(customDevice);
            Layerage clone = clone2.ToLayerage();
            LayerBase.Instances.Add(clone2);
            //

            clone.Children = child.Children;
            child.Children = null;
            LayerageCollection._pasteLayerage(customDevice, clone.Children);
            return clone;
        }

        /// <summary>
        /// Paste layerages ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerages"> The layerages. </param>
        public static IEnumerable<Layerage> PasteLayerages(CanvasDevice customDevice, IEnumerable<Layerage> layerages)
        {
            return from l in layerages select LayerageCollection.PasteLayerage(customDevice, l);
        }


        private static void _pasteLayerage(CanvasDevice customDevice, IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerageCollection._pasteLayerage(customDevice, child.Children);

                //
                ILayer child2 = child.ClipboardSelf;
                ILayer clone2 = child2.Clone(customDevice);
                Layerage clone = clone2.ToLayerage();
                LayerBase.Instances.Add(clone2);
                //

                children[i].Id = clone.Id;
            }
        }

    }
}