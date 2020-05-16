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
        string Title { get; set; }

        Stack<Action> Undos { get; set; }    
    }

    public class IHistoryBase : IHistory
    {
        public string Title { get; set; }

        public Stack<Action> Undos { get; set; } = new Stack<Action>();

        public IHistoryBase(string title)
        {
            this.Title = title;
        }
    }
}