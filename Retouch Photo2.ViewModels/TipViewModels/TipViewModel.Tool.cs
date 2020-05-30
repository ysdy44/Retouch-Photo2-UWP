using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains <see cref="ITool"/> <see cref="IMenu"/> and <see cref="ToolTip.IsOpen"/>
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {        

        /// <summary> Gets or sets the currnet tool. </summary>   
        public ITool Tool
        {
            get => this.tool ;
            set
            {
                if (value == null) return;
                if (this.tool == value) return;

                //The current tool becomes the active tool.
                ITool oldTool = this.tool;
                oldTool.OnNavigatedFrom();

                //The current tool does not become an active tool.
                ITool newTool = value;
                newTool.OnNavigatedTo();

                this.tool = value;
                this.OnPropertyChanged(nameof(this.Tool));//Notify 
            }
        }
        private ITool tool;

        /// <summary> Gets or sets the move tool. </summary>   
        public IMoveTool MoveTool { get; private set; }

        /// <summary> Gets or sets the transformer tool. </summary>   
        public ITransformerTool TransformerTool { get; private set; }

        /// <summary> Gets or sets the create tool. </summary>   
        public ICreateTool CreateTool { get; private set; }


        /// <summary> Gets or sets the all tools. </summary>   
        public IList<ITool> Tools { get; set; } = new List<ITool>();


        /// <summary>
        /// Change tools group value.
        /// </summary>
        /// <param name="currentType"> The current type. </param>
        public void ToolGroupType(ToolType currentType)
        {
            foreach (ITool tool in this.Tools)
            {
                if (tool != null)
                {
                    bool isSelected = (tool.Type == currentType);

                    tool.IsSelected = isSelected;
                }
            }
        }


        /// <summary> Gets or sets the currnet <see cref="TouchbarSlider"/>. </summary>   
        public UIElement TouchbarControl
        {

            get => this.touchbarControl;
            set
            {
                this.touchbarControl = value;
                this.OnPropertyChanged(nameof(this.TouchbarControl));//Notify 
            }
        }
        private UIElement touchbarControl;

    }
}