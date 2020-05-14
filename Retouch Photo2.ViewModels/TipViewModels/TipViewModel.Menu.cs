using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.ComponentModel;

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
        /// Sets the destination IMenu's state is ""FlyoutShow"".
        /// </summary>
        /// <param name="type"> The destination IMenu's type. </param>
        /// <param name="destinations"> The destination state. </param>
        public void ShowMenu(MenuType type)
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
                        default:
                            break;
                    }
                }
            }
        }

    }
}