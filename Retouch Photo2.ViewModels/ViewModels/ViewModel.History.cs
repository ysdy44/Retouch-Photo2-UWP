using Retouch_Photo2.Historys;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the historys. </summary>
        public ObservableCollection<IHistory> Historys { get; private set; } = new ObservableCollection<IHistory>();

        /// <summary> Gets or sets the historys limit count. </summary>
        public int HistorysLimit = 20;

        /// <summary> Gets or sets the availability of undo. </summary>
        public bool IsUndoEnabled
        {
            get => this.isUndoEnabled;
            set
            {
                this.isUndoEnabled = value;
                this.OnPropertyChanged(nameof(IsUndoEnabled));//Notify 
            }
        }
        private bool isUndoEnabled;


        ////////////////////////////////////////

        /// <summary>
        /// Undo the historys.
        /// </summary>
        public bool HistoryUndo()
        {
            int count = this.Historys.Count;
            if (count > 0)
            {
                IHistory history = this.Historys.Last();
                history.Undo();
                this.Historys.Remove(history);

                this.HistoryChanged();
                return true;
            }


            return false;
        }

        /// <summary>
        /// Undo a history into the historys.
        /// </summary>
        /// <param name="history"> The history. </param>
        public void HistoryPush(IHistory history)
        {
            this.Historys.Add(history);
            if (this.Historys.Count > this.HistorysLimit) this.Historys.RemoveAt(0);

            this.HistoryChanged();
        }


        ////////////////////////////////////////


        private void HistoryChanged()
        {
            int count = this.Historys.Count;
            if (count < 1)
            {
                this.IsUndoEnabled = false;
            }
            else
            {
                this.IsUndoEnabled = true;
            }
        }        

    }
}