// Core:              
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:     
using System;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history that contains a ( undo $ redo) method.
    /// </summary>
    public interface IHistory: IDisposable
    {
        /// <summary> Gets or sets the type. </summary>
        HistoryType Type { get; set; }

        /// <summary> Undo method. </summary>
        void Undo();
    }
}