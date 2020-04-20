using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {
        /// <summary> Move the MenuOverlay to a new location in the collection. </summary>
        Action Move { get; set; }
        /// <summary> Occurs when the "flyout" is closed. </summary>
        Action Closed { get; set; }
        /// <summary> Occurs when the "flyout" is opened. </summary>
        Action Opened { get; set; }
        
        /// <summary> Gets IMenu's type. </summary>
        MenuType Type { get; }
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }

        /// <summary> Gets or sets IMenu's postion. </summary>
        Point Postion { get; set; }

        /// <summary> Gets IMenu's layout. </summary>
        FrameworkElement Layout { get; }
        /// <summary> Gets IMenu's button. </summary>
        FrameworkElement Button { get; }

        /// <summary> Gets or sets IMenu's state. </summary>
        MenuState State { get; set; }
        
        /// <summary> Sets IMenu's layer IsHitTestVisible. </summary>
        bool IsHitTestVisible { set; }
    }
}