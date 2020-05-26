using System;
using System.Collections.Generic;

namespace Retouch_Photo2.Historys
{
    public class LayersPropertyHistory : IHistory
    {
        public string Title { get; set; }
        public Stack<Action> UndoActions { get; set; } = new Stack<Action>();

        public void Undo()
        {
            foreach (Action indoAction in UndoActions)
            {
                indoAction();
            }
        }

        public LayersPropertyHistory(string title)
        {
            this.Title = title;
        }
    }
}