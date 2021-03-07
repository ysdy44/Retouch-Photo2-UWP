using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the move tool. </summary>   
        public IMoveTool MoveTool { get; set; } 
        /// <summary> Gets or sets the transformer tool. </summary>   
        public ITransformerTool TransformerTool { get; set; }
        /// <summary> Gets or sets the create tool. </summary>   
        public ICreateTool CreateTool { get; set; }


        /// <summary> Gets or sets the tool type. </summary>
        public ToolType ToolType
        {
            get => this.toolType;
            set
            {
                if (this.toolType == value) return;

                this.toolType = value;
                this.OnPropertyChanged(nameof(ToolType));//Notify  

                foreach (ITool tool in this.Tools)
                {
                    if (tool == null) continue;
                    if (tool.Type == value)
                    {
                        this.Tool = tool;
                    }
                }
            }
        }
        private ToolType toolType = ToolType.None;

        /// <summary> Gets or sets the tool. </summary>   
        public ITool Tool
        {
            get => this.tool;
            private set
            {
                if (this.tool == value) return;

                //The current tool becomes the active tool.
                ITool oldTool = this.tool;
                if (oldTool != null)
                {
                    oldTool.OnNavigatedFrom();
                }

                this.tool = value;
                this.OnPropertyChanged(nameof(Tool));//Notify 
                this.OnPropertyChanged(nameof(ToolIcon));//Notify 
                this.OnPropertyChanged(nameof(ToolPage));//Notify 

                //The current tool does not become an active tool.
                ITool newTool = value;
                if (newTool != null)
                {
                    newTool.OnNavigatedTo();
                }
            }
        }
        private ITool tool = new NoneTool();


        /// <summary> Gets or sets the all tools. </summary>   
        public IList<ITool> Tools { get; set; } = new List<ITool>();


        /// <summary> Gets or sets the tool icon. </summary>
        public ControlTemplate ToolIcon => this.Tool.Icon;

        /// <summary> Gets or sets the tool page. </summary>
        public UIElement ToolPage => this.Tool.Page;

    }
}