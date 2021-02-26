﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            //Setting
            this.SettingViewModel.Setting.Theme = theme2;
            this.SettingViewModel.ConstructTheme();//Construct
            await this.Save();//Write
        }

        private async Task SetType(DeviceLayoutType type2, bool isAdaptive2)
        {
            this.IsAdaptive = isAdaptive2;

            //Setting
            DeviceLayout layout = this.SettingViewModel.Setting.DeviceLayout;
            {
                layout.IsAdaptive = isAdaptive2;
                layout.FallBackType = type2;
                DeviceLayoutType type = layout.GetActualType(this.ActualWidth);
                this.SettingViewModel.DeviceLayoutType = type;
            }
            await this.Save();
        }

        private async Task SetHeight(int height)
        {
            //Setting
            this.SettingViewModel.Setting.LayersHeight = height;
            this.SettingViewModel.ConstructLayersHeight();

            await this.Save();
        }

        private async Task AddMenu(MenuType type)
        {
            //Setting
            this.SettingViewModel.Setting.MenuTypes.Add(type);
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
            await this.Save();
        }

        private async Task RemoveMenu(MenuType type)
        {
            //Setting
            do
            {
                this.SettingViewModel.Setting.MenuTypes.Remove(type);
            }
            while (this.SettingViewModel.Setting.MenuTypes.Contains(type));
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
            await this.Save();
        }

    }
}