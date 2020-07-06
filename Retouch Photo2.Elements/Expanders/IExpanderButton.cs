using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button of <see cref="Expander"/>.
    /// </summary>
    public interface IExpanderButton
    {
        /// <summary> Gets or sets the title. </summary>
        string Title { get; set; }
        /// <summary> Sets the state. </summary>
        ExpanderState ExpanderState { set; }
        /// <summary> Get the self. </summary>
        FrameworkElement Self { get; }

        /// <summary> Sets the center content. </summary>
        object CenterContent { set; }
        /// <summary> Gets the ToolTip. </summary>
        ToolTip ToolTip { get; }
    }
}