using Retouch_Photo2.Elements;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {        
        /// <summary> Gets the type. </summary>
        MenuType Type { get; }

        /// <summary> Gets the expander. </summary>
        IExpander Expander { get; }
    }
}