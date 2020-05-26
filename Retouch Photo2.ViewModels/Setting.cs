using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Retouch_Photo2 's Setting
    /// </summary>
    public class Setting
    {
        /// <summary> <see cref = "Setting" />'s theme. </summary>
        public ElementTheme Theme = ElementTheme.Default;

        /// <summary> <see cref = "Setting" />'s device layout. </summary>
        public DeviceLayout DeviceLayout = DeviceLayout.Default;

        /// <summary> <see cref = "Setting" />'s menu-types. </summary>
        public IList<MenuType> MenuTypes = new List<MenuType>
        {
               //MenuType.Debug,

               MenuType.Edit,
               MenuType.Operate,

               MenuType.Adjustment,
               MenuType.Effect,

               MenuType.Character,
               //MenuType.Paragraph,

               MenuType.Stroke,
               //MenuType.Style,

               //MenuType.History,
               MenuType.Transformer,

               //MenuType.Layer,
               MenuType.Color,
               //MenuType.Keyboard,
        };

    }
}