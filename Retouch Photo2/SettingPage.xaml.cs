using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
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
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //Layout
        PhoneLayout PhoneLayout = new PhoneLayout();
        PadLayout PadLayout = new PadLayout();
        PCLayout PCLayout = new PCLayout();
        DeviceLayoutType DeviceLayoutType
        {
            set
            {
                //Layout
                switch (value)
                {
                    case DeviceLayoutType.Adaptive: this.LayoutBorder.Child = this.PCLayout; break;
                    case DeviceLayoutType.Phone: this.LayoutBorder.Child = this.PhoneLayout; break;
                    case DeviceLayoutType.Pad: this.LayoutBorder.Child = this.PadLayout; break;
                    case DeviceLayoutType.PC: this.LayoutBorder.Child = this.PCLayout; break;
                }

                //Adaptive
                bool isAdaptive = (value == DeviceLayoutType.Adaptive);
                this.AdaptiveTextBlock.Opacity = isAdaptive ? 1.0 : 0.6;
                this.AdaptiveGrid.IsEnabled = isAdaptive;
                this.AdaptiveResetButton.IsEnabled = isAdaptive;
            }
        }


        //@Construct
        public SettingPage()
        {
            this.InitializeComponent();
            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();

            //Theme
            {
                this.LightRadioButton.Tapped += (s, e) =>
                {
                    this.RequestedTheme = ElementTheme.Light;
                    ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Light);
                    this.SettingViewModel.ElementTheme = ElementTheme.Light;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.DarkRadioButton.Tapped += (s, e) =>
                {
                    this.RequestedTheme = ElementTheme.Dark;
                    ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Dark);
                    this.SettingViewModel.ElementTheme = ElementTheme.Dark;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.DefaultRadioButton.Tapped += (s, e) =>
                {
                    this.RequestedTheme = ElementTheme.Default;
                    ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Default);
                    this.SettingViewModel.ElementTheme = ElementTheme.Default;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
            }

            //Layout
            {
                this.PhoneButton.Tapped += (s, e) =>
                {
                    this.DeviceLayoutType = DeviceLayoutType.Phone;
                    this.SettingViewModel.LayoutDeviceType = DeviceLayoutType.Phone;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.PadButton.Tapped += (s, e) =>
                {
                    this.DeviceLayoutType = DeviceLayoutType.Pad;
                    this.SettingViewModel.LayoutDeviceType = DeviceLayoutType.Pad;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.PCButton.Tapped += (s, e) =>
                {
                    this.DeviceLayoutType = DeviceLayoutType.PC;
                    this.SettingViewModel.LayoutDeviceType = DeviceLayoutType.PC;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.AdaptiveButton.Tapped += (s, e) =>
                {
                    this.DeviceLayoutType = DeviceLayoutType.Adaptive;
                    this.SettingViewModel.LayoutDeviceType = DeviceLayoutType.Adaptive;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
            }

            //Adaptive
            {
                this.AdaptiveGrid.ScrollModeChanged += (s, mode) =>
                {
                    this.ScrollViewer.HorizontalScrollMode = mode;
                    this.ScrollViewer.VerticalScrollMode = mode;
                };
                this.AdaptiveGrid.PhoneWidthChanged += (s, value) =>
                {
                    this.SettingViewModel.LayoutPhoneMaxWidth = value;
                    this.SettingViewModel.WriteToLocalFolder();
                };
                this.AdaptiveGrid.PadWidthChanged += (s, value) =>
                {
                    this.SettingViewModel.LayoutPadMaxWidth = value;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
                this.AdaptiveResetButton.Click += (s, e) =>
                {
                    //Adaptive
                    this.AdaptiveGrid.PhoneWidth = SettingViewModel.DefaultLayoutPhoneMaxWidth;
                    this.AdaptiveGrid.PadWidth = SettingViewModel.DefaultLayoutPadMaxWidth;
                    this.AdaptiveGrid.SetWidth();

                    //Setting
                    this.SettingViewModel.LayoutPhoneMaxWidth = SettingViewModel.DefaultLayoutPhoneMaxWidth;
                    this.SettingViewModel.LayoutPadMaxWidth = SettingViewModel.DefaultLayoutPadMaxWidth;
                    this.SettingViewModel.WriteToLocalFolder();//Write
                };
            }

            //Local
            this.LocalFolderButton.Tapped += async (s, e) =>
            {
                IStorageFolder folder = ApplicationData.Current.LocalFolder;
                await Launcher.LaunchFolderAsync(folder);
            };
        }


        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.RequestedTheme = theme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.DefaultRadioButton.IsChecked = (theme == ElementTheme.Default);

            //Layout
            DeviceLayoutType layout = this.SettingViewModel.LayoutDeviceType;
            this.DeviceLayoutType = layout;
            this.PhoneButton.IsChecked = (layout == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (layout == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (layout == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (layout == DeviceLayoutType.Adaptive);

            //Adaptive
            this.AdaptiveGrid.PhoneWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.AdaptiveGrid.PadWidth = this.SettingViewModel.LayoutPadMaxWidth;
            this.AdaptiveGrid.SetWidth();
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
        
    }
}