using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history that contains a ( undo $ redo) method.
    /// </summary>
    public interface IHistory
    {
        /// <summary> Gets IHistory's type. </summary>
        HistoryType Type { get; }

        /// <summary> Gets IHistory's undo. </summary>
        Stack<Action> Undos { get; set; }
        /// <summary> Gets IHistory's redo. </summary>
        //Stack<Action> Redos { get; set; }        
    }

    public class IHistoryBase
    {
        public Stack<Action> Undos { get; set; } = new Stack<Action>();
        //public Stack<Action> Redos { get; set; } = new Stack<Action>();
    }
}