// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {


        #region History


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


        /// <summary>
        /// Undo a history into the historys.
        /// </summary>
        /// <param name="history"> The history. </param>
        public void HistoryPush(IHistory history)
        {
            HistoryBase.Push(history);
            this.IsUndoEnabled = HistoryBase.IsUndoEnabled;
        }


        #endregion


        /// <summary>
        /// Load from a project.
        /// </summary>
        /// <param name="project"> The project. </param>
        public void LoadFromProject(Project project)
        {
            if (project == null) return;

            //Width Height
            this.CanvasTransformer.Width = project.Width;
            this.CanvasTransformer.Height = project.Height;
            
            //Layers
            LayerManager.RootLayerage.Children.Clear();
            if (project.Layerages != null)
            {
                foreach (Layerage layerage in project.Layerages)
                {
                    if (layerage != null)
                    {
                        LayerManager.RootLayerage.Children.Add(layerage);
                    }
                }
            }

            //Arrange
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
        }


        /// <summary>
        /// Indicates that the contents of the CanvasControl need to be redrawn.
        /// </summary>
        /// <param name="mode"> invalidate mode </param>
        public void Invalidate(InvalidateMode mode = InvalidateMode.None) => this.InvalidateAction?.Invoke(mode);
        /// <summary> <see cref = "Action" /> of the <see cref = "ViewModel.Invalidate" />. </summary>
        public Action<InvalidateMode> InvalidateAction { get; set; }


        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}