// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★★★
// Complete:      ★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
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


        //@Converter
        private bool BoolConverter(bool? boolean) => boolean == true;


        //@Construct
        /// <summary>
        /// Initializes a SettingPage. 
        /// </summary>
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            {
                this.Head.LeftButtonClick += (s, e) => this.Frame.GoBack();
                this.Head.RightButtonClick += (s, e) => this.AboutDialog.Show();
                this.ScrollViewer.ViewChanged += (s, e) => this.Head.Move(this.ScrollViewer.VerticalOffset);

                this.ConstructAbout();

                this.ConstructTheme();

                this.ConstructDeviceLayout();
                this.SizeChanged += (s, e) =>
                {
                    if (e.NewSize == e.PreviousSize) return;
                    int width = (int)e.NewSize.Width;
                    this.AdaptiveWidthCountTextBlock.Text = width.ToString();
                };

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