using Retouch_Photo2.Elements;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {        
        /// <summary> Gets the type. </summary>
        MenuType Type { get; }

        /// <summary> Gets or sets the title. </summary>
        string Title { get;  }
        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { get; set; }
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }
        /// <summary> Gets or sets the Self. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets or sets the button. </summary>
        IExpanderButton Button { get; }

        /// <summary>
        /// Calculate postion relative to the placement target.
        /// </summary>
        /// <param name="placementTarget"> The placement target.</param>
        /// <param name="placementMode"> The placement mode.</param>
        void CalculatePostion(FrameworkElement placementTarget, FlyoutPlacementMode placementMode);
        /// <summary> 
        /// Hide the layout. 
        /// </summary>
        void HideLayout();
        /// <summary> 
        /// Hide the layout, and crop the limiting bound.  
        /// </summary>
        void CropLayout();
    }
}