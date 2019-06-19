using Retouch_Photo2.Elements;
using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.Tools.Models;

namespace Retouch_Photo2.TestApp.ViewModels
{
    /// <summary> Retouch_Photo2's the only <see cref = "ViewModel" />. </summary>
    public partial class ViewModel
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


                     

        /// <summary> State of DebugMenuLayout. </summary>
        public MenuLayoutState DebugMenuLayoutState
        {
            get => this.debugMenuLayoutState;
            set
            {
                this.Text = value.ToString();
                if (this.debugMenuLayoutState == value) return;
                this.debugMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.DebugMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState debugMenuLayoutState;
                
        /// <summary> State of EffectMenuLayout. </summary>
        public MenuLayoutState EffectMenuLayoutState
        {
            get => this.effectMenuLayoutState;
            set
            {
                if (this.effectMenuLayoutState == value) return;
                this.effectMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.EffectMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState effectMenuLayoutState;
        
        /// <summary> State of TransformerMenuLayout. </summary>
        public MenuLayoutState TransformerMenuLayoutState
        {
            get => this.transformerMenuLayoutState;
            set
            {
                if (this.transformerMenuLayoutState == value) return;
                this.transformerMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.TransformerMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState transformerMenuLayoutState;

        /// <summary> State of LayerMenuLayout. </summary>
        public MenuLayoutState LayerMenuLayoutState
        {
            get => this.layerMenuLayoutState;
            set
            {
                if (this.layerMenuLayoutState == value) return;
                this.layerMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.LayerMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState layerMenuLayoutState;

        /// <summary> State of ColorMenuLayout. </summary>
        public MenuLayoutState ColorMenuLayoutState
        {
            get => this.colorMenuLayoutState;
            set
            {
                if (this.colorMenuLayoutState == value) return;
                this.colorMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.ColorMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState colorMenuLayoutState;



        
    }
}