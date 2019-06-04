using Retouch_Photo2.TestApp.Tools;
using Retouch_Photo2.TestApp.Tools.Controls;
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

        public ToolsControl()
        {
            this.InitializeComponent();

            this.SetButton(this.RectangleButton, this.ViewModel.Tools[ToolType.Rectangle]);
        }

        private void SetButton(Retouch_Photo2.TestApp.Tools.Button button, Tool tool)
        {
            ToolType type = tool.Type;

            button.Type =type;
            button.CenterContent = tool.Icon;

            button.RootGrid.Tapped += (s, e) =>
            {
                this.ViewModel.ToolType = type;
                this.ViewModel.Tool = this.ViewModel.Tools[type];
            };
        }
    }
}