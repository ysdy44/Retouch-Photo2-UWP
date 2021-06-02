// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★
// Only:              ★★★★★
// Complete:      ★
using Retouch_Photo2.Elements;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Represents a project class with config.
    /// </summary>
    public class Setting
    {
        /// <summary> Gets or sets the theme. </summary>
        public ElementTheme Theme = ElementTheme.Default;

        /// <summary> Gets or sets the device layout. </summary>
        public DeviceLayout DeviceLayout = DeviceLayout.Default;

        /// <summary> Gets or sets the canvas background. </summary>    
        public byte? CanvasBaclground = null;

        /// <summary> Gets or sets the layer height. </summary>    
        public int LayersHeight = 40;

        /// <summary> Gets or sets the menu-types. </summary>
        public IList<MenuType> MenuTypes = new List<MenuType>
        {
             // MenuType.None,

             MenuType.Edit,
             MenuType.Operate,

             MenuType.Adjustment,
             MenuType.Effect,

             MenuType.Stroke,
             // MenuType.Style,

             // MenuType.History,
             MenuType.Transformer,

             // MenuType.Layer
             MenuType.Color
        };

    }
}