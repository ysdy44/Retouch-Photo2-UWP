using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
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
            this.LightRadioButton.Tag = resource.GetString("/$SettingPage/Theme_Light");
            this.DarkRadioButton.Tag = resource.GetString("/$SettingPage/Theme_Dark");
            this.DefaultRadioButton.Tag = resource.GetString("/$SettingPage/Theme_UseSystem");

            this.LayoutTextBlock.Text = resource.GetString("/$SettingPage/Layout");
            this.PhoneButton.Tag = resource.GetString("/$SettingPage/Layout_Phone");
            this.PadButton.Tag = resource.GetString("/$SettingPage/Layout_Pad");
            this.PCButton.Tag = resource.GetString("/$SettingPage/Layout_PC");
            this.AdaptiveButton.Tag = resource.GetString("/$SettingPage/Layout_Adaptive");

            this.AdaptiveTextBlock.Text = resource.GetString("/$SettingPage/Layout_AdaptiveWidth");
            this.AdaptiveResetButton.Content = resource.GetString("/$SettingPage/Layout_ResetAdaptiveWidth");

            this.KeyTextBlock.Text = resource.GetString("/$SettingPage/Key");
            this.IsCenterToggleButton.Tag = resource.GetString("/$SettingPage/Key_IsCenter");
            this.IsRatioToggleButton.Tag = resource.GetString("/$SettingPage/Key_IsRatio");
            this.IsSquareToggleButton.Tag = resource.GetString("/$SettingPage/Key_IsSquare");
            this.IsStepFrequencyToggleButton.Tag = resource.GetString("/$SettingPage/Key_IsStepFrequency");
            this.FullScreenToggleButton.Tag = resource.GetString("/$SettingPage/Key_FullScreen");

            this.LocalTextBlock.Text = resource.GetString("/$SettingPage/Local");
            this.LocalOpenTextBlock.Text = resource.GetString("/$SettingPage/Local_Open");
        }


        //Theme
        private void ConstructTheme()
        {
            this.LightRadioButton.Tapped += (s, e) =>
            {
                ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Light);
                this.SettingViewModel.ElementTheme = ElementTheme.Light;
                this.SettingViewModel.WriteToLocalFolder();//Write
            };
            this.DarkRadioButton.Tapped += (s, e) =>
            {
                ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Dark);
                this.SettingViewModel.ElementTheme = ElementTheme.Dark;
                this.SettingViewModel.WriteToLocalFolder();//Write
            };
            this.DefaultRadioButton.Tapped += (s, e) =>
            {
                ApplicationViewTitleBarBackgroundExtension.SetTheme(ElementTheme.Default);
                this.SettingViewModel.ElementTheme = ElementTheme.Default;
                this.SettingViewModel.WriteToLocalFolder();//Write
            };
        }
        private void NavigatedTheme()
        {
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.DefaultRadioButton.IsChecked = (theme == ElementTheme.Default);
        }


        //Layout
        private void ConstructLayout()
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
        private void NavigatedLayout()
        {
            DeviceLayoutType layout = this.SettingViewModel.LayoutDeviceType;
            this.DeviceLayoutType = layout;
            this.PhoneButton.IsChecked = (layout == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (layout == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (layout == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (layout == DeviceLayoutType.Adaptive);
        }


        //LayoutAdaptive
        private void ConstructLayoutAdaptive()
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
        private void NavigatedLayoutAdaptive()
        {
            this.AdaptiveGrid.PhoneWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.AdaptiveGrid.PadWidth = this.SettingViewModel.LayoutPadMaxWidth;
            this.AdaptiveGrid.SetWidth();
        }


    }
}