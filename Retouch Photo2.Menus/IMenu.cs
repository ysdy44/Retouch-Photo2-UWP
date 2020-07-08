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

        /// <summary> Gets the title. </summary>
        string Title { get; }
        /// <summary> Get  the main page. </summary>
        UIElement MainPage { get; }
        /// <summary> Gets the aecond page. </summary>
        UIElement SecondPage { get; }
        /// <summary> Gets or sets the is aecond page. </summary>
        bool IsSecondPage { get; set;  }
        /// <summary> Reset. </summary>
        void Reset();
        /// <summary> Back. </summary>
        void Back();

        /// <summary> Gets or sets the state. </summary>
        ExpanderState State { get; set; }
        /// <summary> Gets IMenu's placement mode. </summary>
        FlyoutPlacementMode PlacementMode { get; set; }
        /// <summary> Gets the Self. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets the button. </summary>
        IExpanderButton Button { get; }

        /// <summary> Gets or sets the postion X. </summary>
        double PostionX { get; set; }
        /// <summary> Gets or sets the postion Y. </summary>
        double PostionY { get; set; }
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