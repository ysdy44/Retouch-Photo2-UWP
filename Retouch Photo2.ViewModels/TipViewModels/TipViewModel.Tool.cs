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
                this.OnPropertyChanged(nameof(ToolType));//Notify  
            }
        }
        private ToolType toolType = ToolType.Cursor;


    }
}