// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change the order in the arrangement of layerages.
    /// </summary>
    public class LayeragesArrangeHistory : HistoryBase, IHistory
    {

        private readonly Action UndoAction;
        private IList<Layerage> Layerages = new List<Layerage>();

        //@Construct
        /// <summary>
        /// Initializes a LayeragesArrangeHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        /// <param name="layerageCollection"> The layerage-collection. </param>  
        public LayeragesArrangeHistory(string title, LayerageCollection layerageCollection)
        {
            base.Title = title;

            foreach (Layerage  layerage in layerageCollection.RootLayerages)
            {
                this.Layerages.Add(layerage.Clone());
            }

            this.UndoAction = () =>
            {
                layerageCollection.RootLayerages.Clear();
                foreach (Layerage layerage in this.Layerages)
                {
                    layerageCollection.RootLayerages.Add(layerage.Clone());
                }
            };
        }

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            this.UndoAction?.Invoke();
        }
    }
}