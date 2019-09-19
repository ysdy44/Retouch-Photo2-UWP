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
        /// Sets the destination IMenu's state.
        /// </summary>
        /// <param name="type"> The destination IMenu's type. </param>
        /// <param name="destinations"> The destination state. </param>
        public void SetMenuState(MenuType type, MenuState destinations)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {
                    menu.State = destinations;
                    break;
                }
            }
        }

        /// <summary>
        /// Sets the destination IMenu's state.
        /// </summary>
        /// <param name="type"> The destination IMenu's type. </param>
        /// <param name="source"> The source state. </param>
        /// <param name="destinations"> The destination state. </param>
        public void SetMenuState(MenuType type, MenuState source, MenuState destinations)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {
                    if (menu.State == source)
                    {
                        menu.State = destinations;
                    }
                    break;
                }
            }
        }

    }
}