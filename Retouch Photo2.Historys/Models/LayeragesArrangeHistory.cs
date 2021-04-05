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

        private Action UndoAction;
        private IList<Layerage> Layerages = new List<Layerage>();

        //@Construct
        /// <summary>
        /// Initializes a LayeragesArrangeHistory.
        /// </summary>
        /// <param name="type"> The type. </param>  
        public LayeragesArrangeHistory(HistoryType type)
        {
            base.Type = type;

            foreach (Layerage layerage in LayerManager.RootLayerage.Children)
            {
                this.Layerages.Add(layerage.Clone());
            }

            this.UndoAction = () =>
            {
                LayerManager.RootLayerage.Children.Clear();
                foreach (Layerage layerage in this.Layerages)
                {
                    LayerManager.RootLayerage.Children.Add(layerage.Clone());
                }
            };
        }

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            this.UndoAction?.Invoke();
        }

        public void Dispose()
        {
            this.UndoAction = null;

            foreach (Layerage layerage in this.Layerages)
            {
                layerage.Children.Clear();
            }
            this.Layerages.Clear();
            this.Layerages = null;
        }
    }
}