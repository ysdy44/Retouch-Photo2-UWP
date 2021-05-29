using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.Globalization;

namespace Retouch_Photo2
{
    public sealed partial class SettingPage : Page
    {

        private async Task Save()
        {
            await XML.SaveSettingFile(this.SettingViewModel.Setting);
        }


        private async Task SetTheme(ElementTheme theme2)
        {
            // Setting
            this.SettingViewModel.Setting.Theme = theme2;
            this.SettingViewModel.ConstructTheme(); // Construct
            await this.Save(); // Write
        }

        private async Task SetDeviceLayoutType(DeviceLayoutType type2, bool isAdaptive)
        {
            // Setting
            DeviceLayout layout = this.SettingViewModel.Setting.DeviceLayout;
            {
                layout.IsAdaptive = isAdaptive;
                layout.FallBackType = type2;
                DeviceLayoutType type = layout.GetActualType(this.ActualWidth);
                this.SettingViewModel.DeviceLayoutType = type;
            }
            await this.Save();
        }

        private async Task SetLayersHeight(int height)
        {
            // Setting
            this.SettingViewModel.Setting.LayersHeight = height;
            this.SettingViewModel.ConstructLayersHeight();

            await this.Save();
        }

        private async Task SetCanvasBackground(byte? cannel)
        {
            // Setting
            this.SettingViewModel.Setting.CanvasBaclground = cannel;
            this.SettingViewModel.ConstructCanvasBackground();

            await this.Save();
        }

        private async Task AddMenu(MenuType type)
        {
            // Setting
            this.SettingViewModel.Setting.MenuTypes.Add(type);

            IOrderedEnumerable<MenuType> order = this.SettingViewModel.Setting.MenuTypes.OrderBy(t => (int)t);
            this.SettingViewModel.Setting.MenuTypes = order.ToList();
            await this.Save();
        }

        private async Task RemoveMenu(MenuType type)
        {
            // Setting
            do
            {
                this.SettingViewModel.Setting.MenuTypes.Remove(type);
            }
            while (this.SettingViewModel.Setting.MenuTypes.Contains(type));

            IOrderedEnumerable<MenuType> order = this.SettingViewModel.Setting.MenuTypes.OrderBy(t => (int)t);
            this.SettingViewModel.Setting.MenuTypes = order.ToList();
            await this.Save();
        }

        private void SetLanguage(string language)
        {
            if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
            ApplicationLanguages.PrimaryLanguageOverride = language;

            if (string.IsNullOrEmpty(language) == false)
            {
                if (Window.Current.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.Language != language)
                    {
                        frameworkElement.Language = language;
                    }
                }
            }
        }

    }
}