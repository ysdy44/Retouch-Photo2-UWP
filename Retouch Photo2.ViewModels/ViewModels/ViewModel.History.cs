using FanKit.Transformers;
using Retouch_Photo2.Historys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<IHistory> Historys { get; private set; } = new ObservableCollection<IHistory>();
        private int HistoryIndex = 0;        

        public int HistorysLimit = 20;

        public bool IsUndoEnabled
        {
            get => this.isUndoEnabled;
            set
            {
                this.isUndoEnabled = value;
                this.OnPropertyChanged(nameof(this.IsUndoEnabled));//Notify 
            }
        }
        private bool isUndoEnabled;



        public bool Undo()
        {
            int count = this.Historys.Count;
            if (count > 0)
            {
                IHistory history = this.Historys.Last();
                foreach (Action undo in history.Undos)
                {
                    undo();
                }
                this.Historys.Remove(history);

                this.VS();
                return true;
            }


            return false;
        }

        public void Push(IHistory history)
        {
            if (history.Undos.Count == 0) return;

            this.Historys.Add(history);
            if (this.Historys.Count > this.HistorysLimit) this.Historys.RemoveAt(0);

            this.HistoryIndex = this.Historys.Count - 1;

            this.VS();
        }


        private void VS()
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