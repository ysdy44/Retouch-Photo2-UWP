// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★★★
// Complete:      ★★★
using Retouch_Photo2.Elements;
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
    /// Represents a page used to set options.
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        
        bool IsAdaptive
        {
            set
            {
                this.AdaptiveWidthCountTextBlock.Opacity = value ? 1.0 : 0.6;
                this.AdaptiveWidthGrid.IsEnabled = value;
                this.ResetAdaptiveWidthButton.IsEnabled = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a SettingPage. 
        /// </summary>
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
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                int width = (int)e.NewSize.Width;
                this.AdaptiveWidthCountTextBlock.Text = width.ToString();
            };

            this.ConstructLayersHeight();

            this.ConstructMenuType();

            this.LocalFolderButton.Click += async (s, e) =>
            {
                IStorageFolder folder = ApplicationData.Current.LocalFolder;
                await Launcher.LaunchFolderAsync(folder);
            };
        }


        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e) { }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e) { }
    }

    /// <summary> 
    /// Represents a page used to set options.
    /// </summary>
    public sealed partial class SettingPage : Page
    {

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

            //Setting
            this.SettingViewModel.Setting.DeviceLayout.IsAdaptive = isAdaptive2;
            this.SettingViewModel.Setting.DeviceLayout.FallBackType = type2;
            this.SettingViewModel.NotifyDeviceLayoutType();
            await this.Write();
        }

        private async Task SetHeight(int height)
        {
            //Setting
            this.SettingViewModel.Setting.LayersHeight = height;
            LayerManager.ControlsHeight = height;

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