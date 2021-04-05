using System.Collections.ObjectModel;
using System.Linq;

namespace Retouch_Photo2.Historys
{
    public abstract partial class HistoryBase
    {

        //@Static
        /// <summary> Collection <see cref="HistoryBase"/>s instances. </summary>
        public readonly static ObservableCollection<IHistory> Instances = new ObservableCollection<IHistory>();

        /// <summary> Gets or sets the historys limit count. </summary>
        public const int Limit = 20;

        /// <summary> Gets or sets the availability of undo. </summary>
        public static bool IsUndoEnabled
        {
            get
            {
                int count = HistoryBase.Instances.Count;
                if (count < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Pull the historys.
        /// </summary>
        public static bool Pull()
        {
            int count = HistoryBase.Instances.Count;
            if (count > 0)
            {
                IHistory history = HistoryBase.Instances.Last();
                history.Undo();
                HistoryBase.Instances.Remove(history);

                return true;
            }
            else return false;
        }


        /// <summary>
        /// Undo a history into the historys.
        /// </summary>
        /// <param name="history"> The history. </param>
        public static void Push(IHistory history)
        {
            HistoryBase.Instances.Add(history);
            if (HistoryBase.Instances.Count > HistoryBase.Limit) HistoryBase.Instances.RemoveAt(0);
        }

    }
}