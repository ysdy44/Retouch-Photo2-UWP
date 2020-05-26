namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history that contains a ( undo $ redo) method.
    /// </summary>
    public interface IHistory
    {
        string Title { get; set; }

        void Undo();
    }
}