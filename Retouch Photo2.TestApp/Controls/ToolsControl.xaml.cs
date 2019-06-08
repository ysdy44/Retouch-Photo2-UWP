using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ToolsControl" />. 
    /// </summary>
    public sealed partial class ToolsControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        #region DependencyProperty

        /// <summary> 
        /// Type of <see cref = "ToolsControl" />. 
        /// </summary>
        public ToolType ToolType
        {
            get { return (ToolType)GetValue(ToolTypeProperty); }
            set { SetValue(ToolTypeProperty, value); }
        }
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(ToolsControl), new PropertyMetadata(ToolType.None));

        #endregion

        public ToolsControl()
        {
            this.InitializeComponent();
            
            this.SetButton(this.ViewButton, this.ViewModel.ViewTool);
            this.SetButton(this.RectangleButton, this.ViewModel.RectangleTool);
            this.SetButton(this.CursorButton, this.ViewModel.CursorTool);
        }

        private void SetButton(Retouch_Photo2.TestApp.Tools.Button button, Tool tool)
        {
            ToolType type = tool.Type;

            //Content
            button.Type = type;
            button.CenterContent = tool.Icon;

            //ItemClick
            button.RootGrid.Tapped += (s, e) =>
            {
                this.ToolType = type;
                this.ViewModel.Tool = tool;
            };
        }
    }
}