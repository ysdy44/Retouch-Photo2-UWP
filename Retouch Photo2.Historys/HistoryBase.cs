namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history that contains a ( undo $ redo) method.
    /// </summary>
    public abstract class HistoryBase
    {          

        /// <summary> Gets or sets the title. </summary>
        public string Title { get; set; }

        /// <summary> Undo method. </summary>
        public abstract void Undo(); 

    }
}
