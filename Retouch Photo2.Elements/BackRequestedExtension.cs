// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         ★
// Only:              ★★
// Complete:      ★★★
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static member 
    /// for TextBox, Dialog, Layout, Page's BackRequested.
    /// </summary>
    public class BackRequestedExtension
    {
        /// <summary> Gets the SystemNavigationManager.GetForCurrentView.</summary>
        public static SystemNavigationManager Current { get; } = SystemNavigationManager.GetForCurrentView();

        /// <summary> Gets or sets the TextBox's state </summary>
        public static bool TextBoxIsShow = false;
        /// <summary> Gets or sets the Dialog's state </summary>
        public static bool DialogIsShow = false;
        /// <summary> Gets or sets the Layout's state </summary>
        public static bool LayoutIsShow = false;
    }     
}