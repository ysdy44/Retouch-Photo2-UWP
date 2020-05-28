using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {


        /// <summary>
        /// Copy layerages ( form Layerbase to Clipboard).
        /// </summary>
        public static void CopyLayerages(ICanvasResourceCreator resourceCreator, IEnumerable<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                LayerageCollection.CopyLayerage(resourceCreator, layerage);
            }
        }

        /// <summary>
        /// Copy a layerage ( form Layerbase to Clipboard).
        /// </summary>
        public static void CopyLayerage(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            //
            ILayer layerage2 = layerage.Self;
            ILayer clone2 = layerage2.Clone(resourceCreator);
            Clipboard.Instances.Add(clone2);
            //

            LayerageCollection._copyLayerage(resourceCreator, layerage.Children);
        }


        private static void _copyLayerage(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            foreach (Layerage child in children)
            {
                LayerageCollection._copyLayerage(resourceCreator, child.Children);

                //
                ILayer child2 = child.Self;
                ILayer clone2 = child2.Clone(resourceCreator);
                Clipboard.Instances.Add(clone2);
                //
            }
        }



        /// <summary>
        /// Paste layerages ( form Clipboard to Layerbase).
        /// </summary>
        public static IEnumerable<Layerage> PasteLayerages(ICanvasResourceCreator resourceCreator, IEnumerable<Layerage> layerages)
        {
            return from l in layerages select LayerageCollection.PasteLayerage(resourceCreator, l);
        }

        /// <summary>
        /// Paste a layerage ( form Clipboard to Layerbase).
        /// </summary>
        public static Layerage PasteLayerage(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            Layerage child = layerage.Clone();

            //
            ILayer child2 = child.ClipboardSelf;
            ILayer clone2 = child2.Clone(resourceCreator);
            Layerage clone = clone2.ToLayerage();
            LayerBase.Instances.Add(clone2);
            //

            clone.Children = child.Children;
            child.Children = null;
            LayerageCollection._pasteLayerage(resourceCreator, clone.Children);
            return clone;
        }


        private static void _pasteLayerage(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerageCollection._pasteLayerage(resourceCreator, child.Children);

                //
                ILayer child2 = child.ClipboardSelf;
                ILayer clone2 = child2.Clone(resourceCreator);
                Layerage clone = clone2.ToLayerage();
                LayerBase.Instances.Add(clone2);
                //

                children[i] = clone;
            }
        }

    }
}