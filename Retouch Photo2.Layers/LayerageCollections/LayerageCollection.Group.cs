using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        /// <summary>
        /// Remove a layerage from a parents's children,
        /// and insert to parents's parents's children
        /// </summary>
        /// <param name="child"> The child. </param>
        public static bool ReleaseGroupLayer(LayerageCollection layerageCollection, Layerage child)
        {      
            Layerage layerage = child.Parents;
            if (layerage != null)
            {
                IList<Layerage> children = layerageCollection.GetParentsChildren(child);
                IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(layerage);
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
        /// Un group all group layerage
        /// </summary>
        public static void UnGroupAllSelectedLayer(LayerageCollection layerageCollection)
        {
            //Layers
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelectedLayers(layerageCollection);
            Layerage outermost = LayerageCollection.FindOutermost_SelectedLayer(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(outermost);
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
                    IList<Layerage> groupLayerageParentsChildren = layerageCollection.GetParentsChildren(groupLayerage);
                    groupLayerageParentsChildren.Remove(groupLayerage);
                }

            } while (selectedLayerages.Any(l => l.Self.Type == LayerType.Group) == false);
        }


        /// <summary>
        /// Group all selected layers.
        /// </summary>
        public static void GroupAllSelectedLayers(LayerageCollection layerageCollection)
        {     
            //Layers
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelectedLayers(layerageCollection);
            Layerage outermost = LayerageCollection.FindOutermost_SelectedLayer(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;


            //GroupLayer
            TransformerBorder border = new TransformerBorder(selectedLayerages);
            Transformer transformer = border.ToTransformer();

            GroupLayer groupLayer = new GroupLayer
            {
                IsSelected = true,
                IsExpand = false,
                Transform = new Transform(transformer)
            };
            Layerage groupLayerage = groupLayer.ToLayerage();
            groupLayer.Control.ConstructLayerControl(groupLayerage);
            Layer.Instances.Add(groupLayer);


            //Temp
            foreach (Layerage child in selectedLayerages)
            {
                ILayer child2 = child.Self;

                IList<Layerage> childParentsChildren = layerageCollection.GetParentsChildren(child);
                childParentsChildren.Remove(child);
                child2.IsSelected = false;

                groupLayerage.Children.Add(child);
            }

            //Insert
            parentsChildren.Insert(index, groupLayerage);
        }


    }
}