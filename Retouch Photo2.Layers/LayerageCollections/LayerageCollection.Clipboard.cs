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
        /// Copy a layerage ( form Layerbase to Clipboard).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param> 
        /// <param name="layerage"> The layerage. </param>
        public static void CopyLayerage(CanvasDevice canvasDevice, Layerage layerage)
        {
            //
            ILayer layerage2 = layerage.Self;
            ILayer clone2 = layerage2.Clone(canvasDevice);
            Clipboard.Instances.Add(clone2);
            //

            LayerageCollection._copyLayerage(canvasDevice, layerage.Children);
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


        private static void _copyLayerage(CanvasDevice canvasDevice, IList<Layerage> children)
        {
            foreach (Layerage child in children)
            {
                LayerageCollection._copyLayerage(canvasDevice, child.Children);

                //
                ILayer child2 = child.Self;
                ILayer clone2 = child2.Clone(canvasDevice);
                Clipboard.Instances.Add(clone2);
                //
            }
        }

       

        /// <summary>
        /// Paste a layerage ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerage"> The layerage. </param>
        public static Layerage PasteLayerage(CanvasDevice canvasDevice, Layerage layerage)
        {
            Layerage child = layerage.Clone();

            //
            ILayer child2 = child.ClipboardSelf;
            ILayer clone2 = child2.Clone(canvasDevice);
            Layerage clone = clone2.ToLayerage();
            LayerBase.Instances.Add(clone2);
            //

            clone.Children = child.Children;
            child.Children = null;
            LayerageCollection._pasteLayerage(canvasDevice, clone.Children);
            return clone;
        }

        /// <summary>
        /// Paste layerages ( form Clipboard to Layerbase).
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerages"> The layerages. </param>
        public static IEnumerable<Layerage> PasteLayerages(CanvasDevice canvasDevice, IEnumerable<Layerage> layerages)
        {
            return from l in layerages select LayerageCollection.PasteLayerage(canvasDevice, l);
        }


        private static void _pasteLayerage(CanvasDevice canvasDevice, IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerageCollection._pasteLayerage(canvasDevice, child.Children);

                //
                ILayer child2 = child.ClipboardSelf;
                ILayer clone2 = child2.Clone(canvasDevice);
                Layerage clone = clone2.ToLayerage();
                LayerBase.Instances.Add(clone2);
                //

                children[i].Id = clone.Id;
            }
        }

    }
}