// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★★★
// Complete:      ★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private bool BoolConverter(bool? boolean) => boolean == true;
        private string DoubleConverter(double value) => ((int)value).ToString();


        //@Construct
        /// <summary>
        /// Initializes a SettingPage. 
        /// </summary>
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            {
                this.Head.LeftButtonTapped += (s, e) => this.Frame.GoBack();
                this.Head.RightButtonTapped += (s, e) => this.AboutDialog.Show();
                this.ScrollViewer.ViewChanged += (s, e) => this.Head.Move(this.ScrollViewer.VerticalOffset);

                this.ConstructAbout();

                this.ConstructTheme();

                this.ConstructDeviceLayout();

                this.ConstructCanvasBackground();

                this.ConstructLayersHeight();

                this.ConstructMenuType();

                this.ConstructKey();

                this.ConstructLanguage();

                this.LocalFolderButton.Click += async (s, e) =>
                {
                    IStorageFolder folder = ApplicationData.Current.LocalFolder;
                    await Launcher.LaunchFolderAsync(folder);
                };
            }
            this.ConstructStrings();
        }

    }


    public sealed partial class SettingPage : Page
    {

        //@BackRequested
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= BackRequested;
        }
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (BackRequestedExtension.DialogIsShow) return;
            if (BackRequestedExtension.LayoutIsShow) return;

            e.Handled = true;
            this.Frame.GoBack();
        }

    }
}