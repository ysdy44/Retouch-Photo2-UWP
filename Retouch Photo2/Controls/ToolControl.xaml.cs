using Retouch_Photo2.Menus;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ToolControl"/>. 
    /// </summary>
    public sealed partial class ToolControl : UserControl
    {
        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;

        //@Construct
        public ToolControl()
        {
            this.InitializeComponent();
        }
    }
}