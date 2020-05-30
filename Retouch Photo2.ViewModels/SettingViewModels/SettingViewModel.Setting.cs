using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingViewModel" />. 
    /// </summary>
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        //@Construct
        public Setting Setting = new Setting();

        //@Construct
        public void ConstructTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                if (frameworkElement.RequestedTheme != this.Setting.Theme)
                {
                    frameworkElement.RequestedTheme = this.Setting.Theme;
                }
            }
        }

        //@Construct
        public void ConstructLayersHeight()
        {
            LayerageCollection.SetControlHeight(this.Setting.LayersHeight);
        }

        //@Construct
        public void ConstructMenuType(IEnumerable<IMenu> menus)
        {
            foreach (IMenu menu in menus)
            {
                bool isVisible = this.Setting.MenuTypes.Any(m => m == menu.Type);
                menu.Expander.Button.Self.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}