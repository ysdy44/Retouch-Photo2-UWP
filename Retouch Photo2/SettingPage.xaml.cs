using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.SettingPages;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingPage" />. 
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        bool _isParity = false;
        private Style MenuBorderStyle
        {
            get
            {
                this._isParity = !this._isParity;
                return this._isParity ? this.MenuBorderStyle1 : this.MenuBorderStyle2;
            }
        }


        PhoneLayout PhoneLayout = new PhoneLayout();
        PadLayout PadLayout = new PadLayout();
        PCLayout PCLayout = new PCLayout();
        DeviceLayoutType DeviceLayoutType
        {
            set
            {
                switch (value)
                {
                    case DeviceLayoutType.Phone: this.LayoutBorder.Child = this.PhoneLayout; break;
                    case DeviceLayoutType.Pad: this.LayoutBorder.Child = this.PadLayout; break;
                    case DeviceLayoutType.PC: this.LayoutBorder.Child = this.PCLayout; break;
                }
            }
        }
        bool IsAdaptive
        {
            set
            {
                this.AdaptiveTextBlock.Opacity = value ? 1.0 : 0.6;
                this.AdaptiveGrid.IsEnabled = value;
                this.AdaptiveResetButton.IsEnabled = value;
            }
        }


        //@Construct
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.BackButton.Click += (s, e) => this.Frame.GoBack();
            this.AboutButton.Click += (s, e) => this.AboutDialog.Show();
            this.AboutDialog.CloseButton.Click += (s, e) => this.AboutDialog.Hide();
            this.AboutDialog.PrimaryButton.Click += (s, e) => this.AboutDialog.Hide();

            this.ConstructTheme();

            this.ConstructDeviceLayout();

            this.ConstructLayersHeight();

            this.ConstructMenuType();

            this.LocalButton.Click += async (s, e) =>
            {
                IStorageFolder folder = ApplicationData.Current.LocalFolder;
                await Launcher.LaunchFolderAsync(folder);
            };
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e) { }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e) { }


        private async Task Write()
        {
            await XML.SaveSettingFile(this.SettingViewModel.Setting);
        }
        

        private async Task SetTheme(ElementTheme theme2)
        {
            //Setting
            this.SettingViewModel.Setting.Theme = theme2;
            this.SettingViewModel.ConstructTheme();//Construct
            await this.Write();//Write
        }
                       
        private async Task SetType(DeviceLayoutType type2, bool isAdaptive2)
        {
            this.IsAdaptive = isAdaptive2;
            this.DeviceLayoutType = type2;

            //Setting
            this.SettingViewModel.Setting.DeviceLayout.IsAdaptive = isAdaptive2;
            this.SettingViewModel.Setting.DeviceLayout.FallBackType = type2;
            await this.Write();
        }

        private async Task SetHeight(int height)
        {
            //Setting
            this.SettingViewModel.Setting.LayersHeight = height;
            LayerageCollection.SetControlHeight(height);
            await this.Write();
        }

        private async Task AddMenu(MenuType type)
        {
            //Setting
            this.SettingViewModel.Setting.MenuTypes.Add(type);
            this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
            await this.Write();
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
            await this.Write();
        }

    }
}