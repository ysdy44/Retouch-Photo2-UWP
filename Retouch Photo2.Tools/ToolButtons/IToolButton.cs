// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Button of <see cref="ITool"/>.
    /// </summary>
    public interface IToolButton
    {
        /// <summary> Gets the type. </summary>
        ToolType Type { get; set; }
        /// <summary> Gets the title. </summary>
        string Title { get; }
        /// <summary> Gets or sets the IsSelected. </summary>
        bool IsSelected { get; set; }
        /// <summary> Get the self. </summary>
        FrameworkElement Self { get; }

        /// <summary> Sets the icon. </summary>
        object Icon { set; }
        /// <summary> Gets the ToolTip. </summary>
        ToolTip ToolTip { get; }
    }
}