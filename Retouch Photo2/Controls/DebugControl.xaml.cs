using Retouch_Photo2.Menus;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DebugControl" />. 
    /// </summary>
    public sealed partial class DebugControl : Page
    {
        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;

        //@Construct
        public DebugControl()
        {
            this.InitializeComponent();
        }
    }
}