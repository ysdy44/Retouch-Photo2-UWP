using Retouch_Photo2.Elements;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {        
        /// <summary> Gets IMenu's type. </summary>
        MenuType Type { get; }

        /// <summary> Gets IMenu's expander. </summary>
        IExpander Expander { get; }
        /// <summary> Gets IMenu's button. </summary>
        IExpanderButton Button { get; }

        /// <summary> Gets IMenu's button. </summary>
        ExpanderState State { set; }

        /// <summary> Sets IMenu's layer IsHitTestVisible. </summary>
        bool IsHitTestVisible { set; }

        /// <summary> Self. </summary>
        FrameworkElement Self { get; }
    }
}