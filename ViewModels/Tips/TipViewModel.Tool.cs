using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.Tools.Models;
using System.ComponentModel;

namespace ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

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
        private Tool tool = new NoneTool();


        /// <summary> <see cref="Retouch_Photo2.TestApp.Tools.Tool"/>'s ViewTool. </summary>
        public ViewTool ViewTool { get; } = new ViewTool();

        /// <summary> <see cref="Retouch_Photo2.TestApp.Tools.Tool"/>'s RectangleTool. </summary>
        public RectangleTool RectangleTool { get; } = new RectangleTool();

        /// <summary> <see cref="Retouch_Photo2.TestApp.Tools.Tool"/>'s EllipseTool. </summary>
        public EllipseTool EllipseTool { get; } = new EllipseTool();

        /// <summary> <see cref="Retouch_Photo2.TestApp.Tools.Tool"/>'s CursorTool. </summary>
        public CursorTool CursorTool { get; } = new CursorTool();

               


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




    }
}