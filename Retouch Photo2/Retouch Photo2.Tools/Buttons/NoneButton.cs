using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Buttons
{
    /// <summary> 
    /// Button of <see cref = "NoneTool"/>.
    /// </summary>
    public class NoneButton : IToolButton
    {
        public bool IsSelected { private get; set; }

        public FrameworkElement Self => null;

        public ToolButtonType Type => ToolButtonType.None;
    }
}