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
        /// Copy layerages.
        /// </summary>
        public static IEnumerable<Layerage> CopyLayerages(ICanvasResourceCreator resourceCreator, IEnumerable<Layerage> layerages)
        {
            return from l in layerages select LayerageCollection.CopyLayerage(resourceCreator, l);
        }

        /// <summary>
        /// Copy a layerage.
        /// </summary>
        public static Layerage CopyLayerage(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            Layerage child = layerage.Clone();

            //
            ILayer child2 = child.Self;
            ILayer clone2 = child2.Clone(resourceCreator);
            Layerage clone = clone2.ToLayerage();
            Layer.Instances.Add(clone2);
            //

            clone.Children = child.Children;
            child.Children = null;          
            LayerageCollection._copyLayerage(resourceCreator, clone.Children);
            return clone;
        }


        private static void _copyLayerage(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                Layerage child = children[i];
                LayerageCollection._copyLayerage(resourceCreator, child.Children);

                //
                ILayer child2 = child.Self;
                ILayer clone2 = child2.Clone(resourceCreator);
                Layerage clone = clone2.ToLayerage();
                Layer.Instances.Add(clone2);
                //

                children[i] = clone;
            }
        }

    }
}