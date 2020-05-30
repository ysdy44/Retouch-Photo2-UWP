using System;
using System.Collections.Generic;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change layer properties.
    /// </summary>
    public class LayersPropertyHistory : HistoryBase, IHistory
    {
        /// <summary>
        /// Undo actions
        /// </summary>
        public Stack<Action> UndoActions { get; set; } = new Stack<Action>();

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        public LayersPropertyHistory(string title)
        {
            base.Title = title;
        }

        public override void Undo()
        {
            foreach (Action indoAction in UndoActions)
            {
                indoAction?.Invoke();
            }
        }
    }
}