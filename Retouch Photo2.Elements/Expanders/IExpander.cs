using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Interface of <see cref="Expander">.
    /// </summary>
    public interface IExpander
    {
        /// <summary> Occurs when the position changes. </summary>
        Action Move { get; set; }
        /// <summary> Occurs when the flyout closed. </summary>
        Action Closed { get; set; }
        /// <summary> Occurs when the flyout opened. </summary>
        Action Opened { get; set; }
                               
        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { get; set; }        
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }
        /// <summary> Gets or sets the layout. </summary>
        FrameworkElement Layout { get; }
        /// <summary> Gets or sets the button. </summary>
        IExpanderButton Button { get; }

        /// <summary> Hide the Layout. </summary>
        void HideLayout();
        /// <summary> Hide the Layout, and crop the limiting bound.  </summary>
        void CropLayout();
    }
}