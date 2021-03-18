﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    public partial class SettingViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the setting. </summary>
        public Setting Setting { get; set; } = new Setting();


        //@Construct
        /// <summary>
        /// Initializes the all settings.
        /// </summary>
        public void ConstructSetting(Setting setting, IEnumerable<IMenu> menus)
        {
            if (setting == null) return;
            this.Setting = setting;

            //Theme
            this.ConstructTheme();

            //DeviceLayout
            this.ConstructDeviceLayout();
            this.RegisteDeviceLayout();

            //DeviceLayout
            this.ConstructLayersHeight();

            //MenuType
            this.ConstructMenuType(menus);
        }


        //@Construct
        /// <summary>
        /// Initializes the theme.
        /// </summary>
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
        /// <summary>
        /// Initializes the layers-height.
        /// </summary>
        public void ConstructLayersHeight()
        {
            LayerManager.ControlsHeight = this.Setting.LayersHeight;
        }

        //@Construct
        /// <summary>
        /// Initializes the menu-type.
        /// </summary>
        public void ConstructMenuType(IEnumerable<IMenu> menus)
        {
            foreach (IMenu menu in menus)
            {
                bool isVisible = this.Setting.MenuTypes.Any(m => m == menu.Type);
                menu.Button.Self.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}