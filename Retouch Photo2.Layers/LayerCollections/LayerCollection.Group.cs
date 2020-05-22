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
        public static bool ReleaseGroupLayer(LayerCollection layerCollection, Layerage child)
        {      
            Layerage layerage = child.Parents;
            if (layerage != null)
            {
                IList<Layerage> children = layerCollection.GetParentsChildren(child);
                IList<Layerage> parentsChildren = layerCollection.GetParentsChildren(layerage);
                int index = parentsChildren.IndexOf(layerage);
                if (index < 0) index = 0;

                children.Remove(child);
                ILayer layer = child.Self;
                layer.IsSelected = true;

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
            IEnumerable<Layerage> selectedLayerages = LayerCollection.GetAllSelectedLayers(layerCollection);
            Layerage outermost = LayerCollection.FindOutermost_SelectedLayer(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;


            do
            {
                Layerage groupLayerage = selectedLayerages.FirstOrDefault(l => l.Self.Type == LayerType.Group);
                if (groupLayerage == null) break;

                //Insert
                foreach (Layerage layerage in groupLayerage.Children)
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = true;
                    parentsChildren.Insert(index, layerage);
                }
                groupLayerage.Children.Clear();

                //Remove
                {
                    IList<Layerage> groupLayerageParentsChildren = layerCollection.GetParentsChildren(groupLayerage);
                    groupLayerageParentsChildren.Remove(groupLayerage);
                }

            } while (selectedLayerages.Any(l => l.Self.Type == LayerType.Group) == false);
        }


        /// <summary>
        /// Group all selected layers.
        /// </summary>
        public static void GroupAllSelectedLayers(LayerCollection layerCollection)
        {     
            //Layers
            IEnumerable<Layerage> selectedLayerages = LayerCollection.GetAllSelectedLayers(layerCollection);
            Layerage outermost = LayerCollection.FindOutermost_SelectedLayer(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;

            //GroupLayer
            IEnumerable<Transformer> transformers = from l in selectedLayerages select l.Self.GetActualDestinationWithRefactoringTransformer;
            TransformerBorder border = new TransformerBorder(transformers);
            Transformer transformer = border.ToTransformer();
            GroupLayer groupLayer = new GroupLayer
            {
                IsSelected = true,
                IsExpand = false,
                Transform = new Transform(transformer)
            };
            Layer.Instances.Add(groupLayer);
            Layerage groupLayerage = groupLayer.ToLayerage();

            //Temp
            foreach (Layerage child in selectedLayerages)
            {
                ILayer child2 = child.Self;

                IList<Layerage> childParentsChildren = layerCollection.GetParentsChildren(child);
                childParentsChildren.Remove(child);
                child2.IsSelected = false;

                groupLayerage.Children.Add(child);
            }

            //Insert
            parentsChildren.Insert(index, groupLayerage);
        }


    }
}