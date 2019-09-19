using Retouch_Photo2.Tools;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {        
        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.TestApp.Tools.Tool" />. </summary>
        public ITool Tool
        {
            get => this.tool;
            set
            {
                //The current tool becomes the active tool.
                ITool oldTool = this.tool;
                oldTool.OnNavigatedFrom();

                //The current page does not become an active page.
                ITool newTool = value;
                newTool.OnNavigatedTo();

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        private ITool tool;

        /// <summary> TransformerTool. </summary>
        public ITransformerTool TransformerTool;
        

        /// <summary> CursorTool. </summary>
        public ITool CursorTool;

        /// <summary> ViewTool. </summary>
        public ITool ViewTool;

        /// <summary> BrushTool. </summary>
        public ITool BrushTool;

        /// <summary> RectangleTool. </summary>
        public ITool RectangleTool;

        /// <summary> EllipseTool. </summary>
        public ITool EllipseTool;

        /// <summary> PenTool. </summary>
        public ITool PenTool;

        /// <summary> ImageTool. </summary>
        public ITool ImageTool;

        /// <summary> AcrylicTool. </summary>
        public ITool AcrylicTool;

    }
}