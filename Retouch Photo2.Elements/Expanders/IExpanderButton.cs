using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button of <see cref="Expander">.
    /// </summary>
    public interface IExpanderButton
    {
        /// <summary> Occurs when the state changes. </summary>
        Action<ExpanderState> StateChanged { get; set; }

        /// <summary> Gets or sets the center child. </summary>
        object CenterContent { get; set; }
        /// <summary> Gets or sets the tool tip. </summary>
        ToolTip ToolTip { get; }

        /// <summary> Get the self. </summary>
        FrameworkElement Self { get; }

        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { set; }
    }
}