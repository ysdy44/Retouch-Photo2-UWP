using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.SettingPages;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public SettingPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();

            this.ConstructTheme();

            this.ConstructDeviceLayout();

            this.ConstructMenuType();

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


    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SettingPage" />. 
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$SettingPage/Title");

            this.ThemeTextBlock.Text = resource.GetString("/$SettingPage/Theme");
            this.LightRadioButton.Content = resource.GetString("/$SettingPage/Theme_Light");
            this.DarkRadioButton.Content = resource.GetString("/$SettingPage/Theme_Dark");
            this.DefaultRadioButton.Content = resource.GetString("/$SettingPage/Theme_UseSystem");

            this.DeviceLayoutTextBlock.Text = resource.GetString("/$SettingPage/DeviceLayout");
            this.PhoneButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Phone");
            this.PadButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Pad");
            this.PCButton.Content = resource.GetString("/$SettingPage/DeviceLayout_PC");
            this.AdaptiveButton.Content = resource.GetString("/$SettingPage/DeviceLayout_Adaptive");

            this.AdaptiveTextBlock.Text = resource.GetString("/$SettingPage/DeviceLayout_AdaptiveWidth");
            this.AdaptiveResetButton.Content = resource.GetString("/$SettingPage/DeviceLayout_ResetAdaptiveWidth");

            this.KeyTextBlock.Text = resource.GetString("/$SettingPage/Key");
            this.IsCenterToggleButton.Content = resource.GetString("/$SettingPage/Key_IsCenter");
            this.IsRatioToggleButton.Content = resource.GetString("/$SettingPage/Key_IsRatio");
            this.IsSquareToggleButton.Content = resource.GetString("/$SettingPage/Key_IsSquare");
            this.IsStepFrequencyToggleButton.Content = resource.GetString("/$SettingPage/Key_IsStepFrequency");
            this.FullScreenToggleButton.Content = resource.GetString("/$SettingPage/Key_FullScreen");

            this.MenuTypeTextBlock.Text = resource.GetString("/$SettingPage/MenuType");
            this.MenuTypeTipTextBlock.Text = resource.GetString("/$SettingPage/MenuTypeTip");

            this.LocalTextBlock.Text = resource.GetString("/$SettingPage/Local");
            this.LocalOpenTextBlock.Text = resource.GetString("/$SettingPage/Local_Open");
        }



        //Theme
        private void ConstructTheme()
        {
            ElementTheme theme = this.SettingViewModel.Setting.Theme;
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.DefaultRadioButton.IsChecked = (theme == ElementTheme.Default);


            async Task setTheme(ElementTheme theme2)
            {
                //Setting
                this.SettingViewModel.Setting.Theme = theme2;
                this.SettingViewModel.ConstructTheme();//Construct
                await this.Write();//Write
            };


            this.LightRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Light);
            this.DarkRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Dark);
            this.DefaultRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Default);
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


        //DeviceLayout
        private void ConstructDeviceLayout()
        {
            DeviceLayout deviceLayout = this.SettingViewModel.Setting.DeviceLayout;
            this.ConstructDeviceLayoutType(deviceLayout.FallBackType, deviceLayout.IsAdaptive);
            this.ConstructDeviceLayoutAdaptive(deviceLayout.PhoneMaxWidth, deviceLayout.PadMaxWidth);
        }


        private void ConstructDeviceLayoutType(DeviceLayoutType type, bool isAdaptive)
        {
            this.DeviceLayoutType = type;
            this.IsAdaptive = isAdaptive;
            this.PhoneButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (isAdaptive);


            async Task setType(DeviceLayoutType type2, bool isAdaptive2)
            {
                this.IsAdaptive = isAdaptive2;
                this.DeviceLayoutType = type2;

                //Setting
                this.SettingViewModel.Setting.DeviceLayout.IsAdaptive = isAdaptive2;
                this.SettingViewModel.Setting.DeviceLayout.FallBackType = type2;
                await this.Write();
            }


            this.PhoneButton.Tapped += async (s, e) => await setType(DeviceLayoutType.Phone, false);
            this.PadButton.Tapped += async (s, e) => await setType(DeviceLayoutType.Pad, false);
            this.PCButton.Tapped += async (s, e) => await setType(DeviceLayoutType.PC, false);
            this.AdaptiveButton.Tapped += async (s, e) => await setType(DeviceLayoutType.PC, true);
        }


        private void ConstructDeviceLayoutAdaptive(int phone, int pad)
        {
            this.AdaptiveGrid.PhoneWidth = phone;
            this.AdaptiveGrid.PadWidth = pad;
            this.AdaptiveGrid.SetWidth();


            this.AdaptiveGrid.ScrollModeChanged += (s, mode) =>
            {
                this.ScrollViewer.HorizontalScrollMode = mode;
                this.ScrollViewer.VerticalScrollMode = mode;
            };
            this.AdaptiveGrid.PhoneWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PhoneMaxWidth = value;
                await this.Write();
            };
            this.AdaptiveGrid.PadWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = value;
                await this.Write();
            };


            this.AdaptiveResetButton.Click += async (s, e) =>
            {
                DeviceLayout default2 = DeviceLayout.Default;
                int phone2 = default2.PhoneMaxWidth;
                int pad2 = default2.PadMaxWidth;

                this.AdaptiveGrid.PhoneWidth = phone2;
                this.AdaptiveGrid.PadWidth = pad2;
                this.AdaptiveGrid.SetWidth();

                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PhoneMaxWidth = phone2;
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = pad2;
                await this.Write();//Write
            };
        }



        //MenuType
        private void ConstructMenuType()
        {
            bool isParity = false;
            IList<MenuType> menuTypes = this.SettingViewModel.Setting.MenuTypes;

            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                bool isVisible = menuTypes.Any(m => m == menu.Type);


                CheckBox checkBox = new CheckBox
                {
                    Content = menu.Expander.Title,
                    IsChecked = isVisible,
                };
                checkBox.Checked += async (s, e) =>
                {
                    //Setting
                    this.SettingViewModel.Setting.MenuTypes.Add(menu.Type);
                    this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
                    await this.Write();
                };
                checkBox.Unchecked += async (s, e) =>
                {
                    //Setting
                    do
                    {
                        this.SettingViewModel.Setting.MenuTypes.Remove(menu.Type);
                    }
                    while (this.SettingViewModel.Setting.MenuTypes.Contains(menu.Type));
                    this.SettingViewModel.ConstructMenuType(this.TipViewModel.Menus);
                    await this.Write();
                };


                isParity = !isParity;
                this.MenusStackPanel.Children.Add(new Border
                {
                    Child = checkBox,
                    Style = isParity ? this.MenuBorderStyle1 : this.MenuBorderStyle2,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                });
            }
        }



    }
}