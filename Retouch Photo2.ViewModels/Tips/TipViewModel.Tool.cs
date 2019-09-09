using Retouch_Photo2.Tools;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels.Tips
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {
        private void SetToolIsOpen(bool isOpen)
        {
            switch (this.Tool.Type)
            {
                case ToolType.None:
                    break;

                case ToolType.Cursor:
                   this.CursorTool.IsOpen = isOpen;
                    break;

                case ToolType.View:
              this.ViewTool.IsOpen = isOpen;
                    break;

                case ToolType.Rectangle:
                    break;

                case ToolType.Ellipse:
                    break;

                case ToolType.Pen:
                 this.PenTool.IsOpen = isOpen;
                    break;

                case ToolType.Acrylic:
                    break;
            } 
        }

        
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