using System.Collections.Generic;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the menus. </summary>
        public readonly IList<MenuViewModel> Menus = new List<MenuViewModel>
        {
            new MenuViewModel{ Type = "Edit" },
            new MenuViewModel{ Type = "Operate" },

            new MenuViewModel{ Type = "Adjustment" },
            new MenuViewModel{ Type = "Effect" },

            new MenuViewModel{ Type = "Text" },
            new MenuViewModel{ Type = "Stroke" },
            new MenuViewModel{ Type = "Style" },

            new MenuViewModel{ Type = "History" },
            new MenuViewModel{ Type = "Transformer" },
            new MenuViewModel{ Type = "Layer" },

            new MenuViewModel{ Type = "Color" },
        };        
    }
}