using Retouch_Photo2.Tools;
using System.ComponentModel;

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
                    this.IsCursorToolOpen = isOpen;
                    break;

                case ToolType.View:
                    this.IsViewToolOpen = isOpen;
                    break;

                case ToolType.Rectangle:
                    break;

                case ToolType.Ellipse:
                    break;

                case ToolType.Acrylic:
                    break;
            } 
        }


        ///////////////////////////////////////////////////////////////////////////////////


        /// <summary> Retouch_Photo2's the only <see cref = "Retouch_Photo2.TestApp.Tools.Tool" />. </summary>
        public Tool Tool
        {
            get => this.tool;
            set
            {
                //The current tool becomes the active tool.
                Tool oldTool = this.tool;
                oldTool.ToolOnNavigatedFrom();

                //The current page does not become an active page.
                Tool newTool = value;
                newTool.ToolOnNavigatedTo();

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        private Tool tool;

        /// <summary> <see cref="Retouch_Photo2.Tools.ITransformerTool"/>'s TransformerTool. </summary>
        public ITransformerTool TransformerTool;



        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s CursorTool. </summary>
        public Tool CursorTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s ViewTool. </summary>
        public Tool ViewTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s BrushTool. </summary>
        public Tool BrushTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s RectangleTool. </summary>
        public Tool RectangleTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s EllipseTool. </summary>
        public Tool EllipseTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s ImageTool. </summary>
        public Tool ImageTool;

        /// <summary> <see cref="Retouch_Photo2.Tools.Tool"/>'s AcrylicTool. </summary>
        public Tool AcrylicTool;
        

        //////////////////////////////////////////////////////////////////////////////////////



        /// <summary> IsOpen of the <see cref = "TipViewModel.CursorTool" />. </summary>
        public bool IsCursorToolOpen
        {
            get => this.isCursorToolOpen;
            set
            {
                this.isCursorToolOpen = value;
                this.OnPropertyChanged(nameof(this.IsCursorToolOpen));//Notify 
            }
        }
        private bool isCursorToolOpen;

        /// <summary> IsOpen of the <see cref = "TipViewModel.ViewTool" />. </summary>
        public bool IsViewToolOpen
        {
            get => this.isViewToolOpen;
            set
            {
                this.isViewToolOpen = value;
                this.OnPropertyChanged(nameof(this.IsViewToolOpen));//Notify 
            }
        }
        private bool isViewToolOpen;        


    }
}