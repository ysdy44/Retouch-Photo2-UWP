using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Menus. </summary>
        public IList<IMenu> Menus { get; set; } = new List<IMenu>();


        /// <summary>
        /// Show the layout.
        /// </summary>
        /// <param name="type"> The menu type. </param>
        public void ShowMenuLayout(MenuType type = MenuType.Layer)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {
                    switch (menu.Expander.State)
                    {
                        case ExpanderState.Hide:
                            menu.Expander.State = ExpanderState.FlyoutShow;
                            break;
                        case ExpanderState.OverlayNotExpanded:
                            menu.Expander.State = ExpanderState.Overlay;
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Show the layout relative to the placement target.
        /// </summary>
        /// <param name="type"> The menu type. </param>
        /// <param name="placementTarget"> The  placement target.</param>
        /// <param name="placementMode"> The  placement mode.</param>
        public void ShowMenuLayoutAt(MenuType type, FrameworkElement placementTarget, FlyoutPlacementMode placementMode = FlyoutPlacementMode.Top)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {

                    if (menu.Expander.State == ExpanderState.Hide)
                    {
                        menu.Expander.CalculatePostion(placementTarget, placementMode);

                        menu.Expander.State = ExpanderState.FlyoutShow;
                    }

                }
            }
        }

    }
}