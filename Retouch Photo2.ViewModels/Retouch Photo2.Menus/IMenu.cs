using Retouch_Photo2.Elements;

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
    }
}