using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;

namespace Retouch_Photo2.Historys
{
    public class LayeragesHistory : IHistory
    {
        public string Title { get; set; }
        public Action UndoAction { get; set; }

        public IList<Layerage> Layerages { get; set; } = new List<Layerage>();

        public void Undo()
        {
            UndoAction();
        }
        public LayeragesHistory(string title)
        {
            this.Title = title;
        }
    }

}