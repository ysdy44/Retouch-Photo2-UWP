using Retouch_Photo2.Elements;
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

        private async Task SetDeviceLayoutType(DeviceLayoutType type2, bool isAdaptive)
        {
            //Setting
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
            //Setting
            this.SettingViewModel.Setting.LayersHeight = height;
            this.SettingViewModel.ConstructLayersHeight();

            await this.Save();
        }

        private async Task SetCanvasBackground(byte? cannel)
        {
            //Setting
            this.SettingViewModel.Setting.CanvasBaclground = cannel;
            this.SettingViewModel.ConstructCanvasBackground();

            await this.Save();
        }

        private async Task AddMenu(string type)
        {
            //Setting
            this.SettingViewModel.Setting.MenuTypes.Add(type);
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
            await this.Save();
        }

        private async Task RemoveMenu(string type)
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