using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Button of <see cref="ITool"/>.
    /// </summary>
    public interface IToolButton
    {
        /// <summary> Gets or sets the title. </summary>
        string Title { get; set; }
        /// <summary> Gets or sets the IsSelected. </summary>
        bool IsSelected { get; set; }
        /// <summary> Get the self. </summary>
        FrameworkElement Self { get; }

        /// <summary> Sets the center content. </summary>
        object CenterContent { set; }
        /// <summary> Gets the ToolTip. </summary>
        ToolTip ToolTip { get; }
    }
}