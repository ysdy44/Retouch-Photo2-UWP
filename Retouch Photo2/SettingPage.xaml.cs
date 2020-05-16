using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.SettingPages;
using Retouch_Photo2.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();

            this.ConstructTheme(this.SettingViewModel.Setting.Theme);

            this.ConstructDeviceLayout(this.SettingViewModel.Setting.DeviceLayout);

            this.ConstructMenuType(this.SettingViewModel.Setting.MenuTypes);

            this.LocalButton.Tapped += async (s, e) =>
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

    }
}