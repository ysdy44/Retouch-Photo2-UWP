using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Gets all photocopiers, which in ( layer and children )'s style manager.
        /// </summary>
        /// <returns> The yield photocopiers. </returns>
        public static IEnumerable<Photocopier> GetPhotocopiers(IEnumerable<Layerage> layers)
        {
            foreach (Layerage child in layers)
            {
                ILayer child2 = child.Self;

                foreach (Photocopier photocopier in LayerageCollection.GetPhotocopiers(child.Children))
                {
                    yield return photocopier;
                }

                //ImageLayer
                if (child2.Type == LayerType.Image)
                {
                    ImageLayer imageLayer = (ImageLayer)child2;
                    yield return imageLayer.Photocopier;
                }

                //Fill
                if (child2.Style.Fill.Type == BrushType.Image)
                {
                    yield return child2.Style.Fill.Photocopier;
                }
                //Stroke
                if (child2.Style.Stroke.Type == BrushType.Image)
                {
                    yield return child2.Style.Stroke.Photocopier;
                }
            }
        }
        

        /// <summary>
        /// Gets all layerages
        /// </summary>
        /// <returns> The yield layerages. </returns>
        public static IEnumerable<Layerage> GetLayerages(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage child in layerages)
            {
                yield return child;

                foreach (Layerage photocopier in LayerageCollection.GetLayerages(child.Children))
                {
                    yield return photocopier;
                }
            }
        }


    }
}