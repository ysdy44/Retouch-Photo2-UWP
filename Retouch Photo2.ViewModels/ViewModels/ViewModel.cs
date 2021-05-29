// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools;
using System;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        #region Tool


        /// <summary> Gets or sets the move tool. </summary>   
        public IMoveTool MoveTool { get; set; }
        /// <summary> Gets or sets the transformer tool. </summary>   
        public ITransformerTool TransformerTool { get; set; }
        /// <summary> Gets or sets the create tool. </summary>   
        public ICreateTool CreateTool { get; set; }
        /// <summary> Gets or sets the clicke tool. </summary>   
        public IClickeTool ClickeTool { get; set; }



        /// <summary> Gets or sets the tool type. </summary>
        public ToolType ToolType
        {
            get => this.toolType;
            set
            {
                if (this.toolType == value) return;

                this.toolType = value;
                this.OnPropertyChanged(nameof(ToolType)); // Notify  
            }
        }
        private ToolType toolType = ToolType.Cursor;


        #endregion


        #region History


        /// <summary> Gets or sets the availability of undo. </summary>
        public bool IsUndoEnabled
        {
            get => this.isUndoEnabled;
            set
            {
                this.isUndoEnabled = value;
                this.OnPropertyChanged(nameof(IsUndoEnabled)); // Notify 
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

            // Width Height
            this.CanvasTransformer.Width = project.Width;
            this.CanvasTransformer.Height = project.Height;
            
            // Layers
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

            // Arrange
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