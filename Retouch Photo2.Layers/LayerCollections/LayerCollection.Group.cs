using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Remove a layer from a parents's children,
        /// and insert to parents's parents's children
        /// </summary>
        /// <param name="layer"> The layer. </param>
        public static bool ReleaseGroupLayer(LayerCollection layerCollection, ILayer child)
        {
            ILayer layer = child.Parents;
            if (layer != null)
            {
                IList<ILayer> children = layerCollection.GetParentsChildren(child);
                IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(layer);
                int index = parentsChildren.IndexOf(layer);
                if (index < 0) index = 0;

                children.Remove(child);
                child.IsSelected = true;

                parentsChildren.Insert(index, child);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Un group all group layer
        /// </summary>
        /// <param name="groupLayer"> The group layer. </param>
        public static void UnGroupAllSelectedLayer(LayerCollection layerCollection)
        {
            //Layers
            IEnumerable<ILayer> selectedLayers = LayerCollection.GetAllSelectedLayers(layerCollection);
            ILayer outermost = LayerCollection.FindOutermost_SelectedLayer(selectedLayers);
            if (outermost == null) return;
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;


            do
            {
                ILayer groupLayer = selectedLayers.FirstOrDefault(l => l.Type == LayerType.Group);
                if (groupLayer == null) break;

                //Insert
                foreach (ILayer child in groupLayer.Children)
                {
                    child.IsSelected = true;
                    parentsChildren.Insert(index, child);
                }
                groupLayer.Children.Clear();

                //Remove
                {
                    IList<ILayer> groupLayerParentsChildren = layerCollection.GetParentsChildren(groupLayer);
                    groupLayerParentsChildren.Remove(groupLayer);
                }

            } while (selectedLayers.Any(l => l.Type == LayerType.Group) == false);
        }


        /// <summary>
        /// Group all selected layers.
        /// </summary>
        public static void GroupAllSelectedLayers(LayerCollection layerCollection)
        {
            //Layers
            IEnumerable<ILayer> selectedLayers = LayerCollection.GetAllSelectedLayers(layerCollection);
            ILayer outermost = LayerCollection.FindOutermost_SelectedLayer(selectedLayers);
            if (outermost == null) return;
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;

            //GroupLayer
            IEnumerable<Transformer> transformers = from l in selectedLayers select l.GetActualDestinationWithRefactoringTransformer;
            TransformerBorder border = new TransformerBorder(transformers);
            Transformer transformer = border.ToTransformer();
            GroupLayer groupLayer = new GroupLayer
            {
                IsSelected = true,
                IsExpand = false,
                Transform = new Transform(transformer)
            };

            //Temp
            foreach (ILayer child in selectedLayers)
            {
                IList<ILayer> childParentsChildren = layerCollection.GetParentsChildren(child);
                childParentsChildren.Remove(child);

                child.IsSelected = false;
                groupLayer.Children.Add(child);
            }

            //Insert
            parentsChildren.Insert(index, groupLayer);
        }


    }
}