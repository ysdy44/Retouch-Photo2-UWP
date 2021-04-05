// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              
// Complete:      
namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history that contains a ( undo $ redo) method.
    /// </summary>
    public abstract partial class HistoryBase
    {
        /// <summary> Gets or sets the type. </summary>
        public HistoryType Type { get; set; }

        /// <summary> Undo method. </summary>
        public abstract void Undo();
    }
}