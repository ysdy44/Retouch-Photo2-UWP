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
                    this.IsCursorToolOpen = isOpen;
                    break;

                case ToolType.View:
                    this.IsViewToolOpen = isOpen;
                    break;

                case ToolType.Rectangle:
                    break;

                case ToolType.Ellipse:
                    break;

                case ToolType.Pen:
                    this.IsPenToolOpen = isOpen;
                    break;

                case ToolType.Acrylic:
                    break;
            } 
        }


        ///////////////////////////////////////////////////////////////////////////////////


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

        /// <summary> IsOpen of the <see cref = "TipPenModel.PenTool" />. </summary>
        public bool IsPenToolOpen
        {
            get => this.isPenToolOpen;
            set
            {
                this.isPenToolOpen = value;
                this.OnPropertyChanged(nameof(this.IsPenToolOpen));//Notify 
            }
        }
        private bool isPenToolOpen;

    }
}