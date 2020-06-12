using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Remove a layerage from a parents's children,
        /// and insert to parents's parents's children
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <param name="layerage"> The layerage. </param>
        public static bool ReleaseGroupLayer(LayerageCollection layerageCollection, Layerage layerage)
        {
            Layerage parents = layerage.Parents;

            if (parents != null)
            {
                ILayer parents2 = parents.Self;

                IList<Layerage> parentsParentsChildren = layerageCollection.GetParentsChildren(parents);
                IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(layerage);
                int parentsIndex = parentsChildren.IndexOf(parents);
                if (parentsIndex < 0) parentsIndex = 0;
                if (parentsIndex > parentsParentsChildren.Count - 1) parentsIndex = parentsParentsChildren.Count - 1;

                parentsChildren.Remove(layerage);
                parentsParentsChildren.Insert(parentsIndex, layerage);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Un group all group layerage
        /// </summary>
        public static void UnGroupAllSelectedLayer(LayerageCollection layerageCollection)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected(layerageCollection);
            Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;


            do
            {
                Layerage groupLayerage = selectedLayerages.FirstOrDefault(l => l.Self.Type == LayerType.Group);
                if (groupLayerage == null) break;
                ILayer groupLayer = groupLayerage.Self;

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
        /// Group all selected layerages.
        /// </summary>     
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public static void GroupAllSelectedLayers(CanvasDevice customDevice, LayerageCollection layerageCollection)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected(layerageCollection);
            Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayerages);
            if (outermost == null) return;
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(outermost);
            int index = parentsChildren.IndexOf(outermost);
            if (index < 0) index = 0;


            //GroupLayer
            GroupLayer groupLayer = new GroupLayer(customDevice)
            {
                IsSelected = true,
                IsExpand = false,
                //Refactoring
                IsRefactoringTransformer = true,
            };
            Layerage groupLayerage = groupLayer.ToLayerage();
            LayerBase.Instances.Add(groupLayer);


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