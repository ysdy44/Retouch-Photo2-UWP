using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// Base of <see cref="ITool"/>.
    /// </summary>
    public class ToolBase 
    {

        //@Static  
        /// <summary> A border, contains a <see cref="ITool.Icon"/>. </summary>
        public static Border IconBorder = new Border();
        /// <summary> A border, contains a <see cref="ITool.Page"/>. </summary>
        public static Border PageBorder = new Border();
        /// <summary> A instance of <see cref="ITool"/>. </summary>
        public static ITool Instance
        {
            get => ToolBase.instance;
            set
            {
                if (value == null) return;
                if (ToolBase.instance == value) return;

                //The current tool becomes the active tool.
                ITool oldTool = ToolBase.instance;
                oldTool.OnNavigatedFrom();
                if (oldTool.Button != null) oldTool.Button.IsSelected = false;

                //The current tool does not become an active tool.
                ITool newTool = value;
                newTool.OnNavigatedTo();
                if (newTool.Button != null) newTool.Button.IsSelected = true;

                ToolBase.IconBorder.Child = value.Icon;
                ToolBase.PageBorder.Child = value.Page;
                ToolBase.instance = value;
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