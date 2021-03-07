// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              
// Complete:      ★★
using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Manager of <see cref="ITool"/>.
    /// </summary>
    public static class ToolManager
    {

        //@Static  
        /// <summary> A control, contains a <see cref="ITool.Icon"/>. </summary>
        public static ContentControl IconControl { get; } = new ContentControl();
        /// <summary> A border, contains a <see cref="ITool.Page"/>. </summary>
        public static Border PageBorder { get; } = new Border();
        /// <summary> A instance of <see cref="ITool"/>. </summary>
        public static ITool Instance
        {
            get => ToolManager.instance;
            set
            {
                if (value == null) return;
                if (ToolManager.instance == value) return;

                //The current tool becomes the active tool.
                ITool oldTool = ToolManager.instance;
                oldTool.OnNavigatedFrom();
                oldTool.IsSelected = false;

                //The current tool does not become an active tool.
                ITool newTool = value;
                newTool.OnNavigatedTo();
                newTool.IsSelected = true;

                ToolManager.IconControl.Template = value.Icon;
                ToolManager.PageBorder.Child = value.Page;
                ToolManager.instance = value;
            }
        }
        private static ITool instance = new NoneTool();


        //@Static  
        /// <summary> Gets or sets the move tool. </summary>   
        public static IMoveTool MoveTool { get; } = new MoveTool();
        /// <summary> Gets or sets the transformer tool. </summary>   
        public static ITransformerTool TransformerTool { get; } = new TransformerTool();
        /// <summary> Gets or sets the create tool. </summary>   
        public static ICreateTool CreateTool { get; } = new CreateTool();
        
    }
}