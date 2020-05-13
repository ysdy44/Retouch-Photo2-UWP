using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Threading.Tasks;
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
            this.LightRadioButton.Content = resource.GetString("/$SettingPage/Theme_Light");
            this.DarkRadioButton.Content = resource.GetString("/$SettingPage/Theme_Dark");
            this.DefaultRadioButton.Content = resource.GetString("/$SettingPage/Theme_UseSystem");

            this.LayoutTextBlock.Text = resource.GetString("/$SettingPage/Layout");
            this.PhoneButton.Content = resource.GetString("/$SettingPage/Layout_Phone");
            this.PadButton.Content = resource.GetString("/$SettingPage/Layout_Pad");
            this.PCButton.Content = resource.GetString("/$SettingPage/Layout_PC");
            this.AdaptiveButton.Content = resource.GetString("/$SettingPage/Layout_Adaptive");

            this.AdaptiveTextBlock.Text = resource.GetString("/$SettingPage/Layout_AdaptiveWidth");
            this.AdaptiveResetButton.Content = resource.GetString("/$SettingPage/Layout_ResetAdaptiveWidth");

            this.KeyTextBlock.Text = resource.GetString("/$SettingPage/Key");
            this.IsCenterToggleButton.Content = resource.GetString("/$SettingPage/Key_IsCenter");
            this.IsRatioToggleButton.Content = resource.GetString("/$SettingPage/Key_IsRatio");
            this.IsSquareToggleButton.Content = resource.GetString("/$SettingPage/Key_IsSquare");
            this.IsStepFrequencyToggleButton.Content = resource.GetString("/$SettingPage/Key_IsStepFrequency");
            this.FullScreenToggleButton.Content = resource.GetString("/$SettingPage/Key_FullScreen");

            this.LocalTextBlock.Text = resource.GetString("/$SettingPage/Local");
            this.LocalOpenTextBlock.Text = resource.GetString("/$SettingPage/Local_Open");
        }


        //Theme
        private void ConstructTheme()
        {
            async Task setTheme(ElementTheme theme)
            {
                this.SettingViewModel.Theme = theme;
                await this.WriteToLocalFolder();//Write
            };

            this.LightRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Light);
            this.DarkRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Dark);
            this.DefaultRadioButton.Tapped += async (s, e) => await setTheme(ElementTheme.Default);
        }
        private void NavigatedTheme( ElementTheme theme )
        {
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.DefaultRadioButton.IsChecked = (theme == ElementTheme.Default);
        }


        //Layout
        private void ConstructLayout()
        {
            async Task setLayout(bool isAdaptive, DeviceLayoutType type)
            {
                DeviceLayout layout = new DeviceLayout
                {
                    IsAdaptive = isAdaptive,
                    FallBackType = type,
                    PadMaxWidth = this.SettingViewModel.DeviceLayout.PadMaxWidth,
                    PhoneMaxWidth = this.SettingViewModel.DeviceLayout.PhoneMaxWidth,
                };

                this.DeviceLayout = layout;
                this.SettingViewModel.DeviceLayout = layout;
                await this.WriteToLocalFolder();
            }

            this.PhoneButton.Tapped += async (s, e) => await setLayout(false, DeviceLayoutType.Phone);
            this.PadButton.Tapped += async (s, e) => await setLayout(false, DeviceLayoutType.Pad);
            this.PCButton.Tapped += async (s, e) => await setLayout(false, DeviceLayoutType.PC);
            this.AdaptiveButton.Tapped += async (s, e) => await setLayout(true, DeviceLayoutType.PC);
        }
        private void NavigatedLayout(DeviceLayout  layout)
        {
            this.DeviceLayout = layout;

            this.PhoneButton.IsChecked = (layout.IsAdaptive == false && layout.FallBackType == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (layout.IsAdaptive == false && layout.FallBackType == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (layout.IsAdaptive == false && layout.FallBackType == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (layout.IsAdaptive);
        }


        //LayoutAdaptive
        private void ConstructLayoutAdaptive()
        {
            async Task setPhoneWidth(int width)
            {
                this.SettingViewModel.DeviceLayout = new DeviceLayout
                {
                    FallBackType = this.SettingViewModel.DeviceLayout.FallBackType,
                    PhoneMaxWidth = width,
                    PadMaxWidth = this.SettingViewModel.DeviceLayout.PadMaxWidth,
                };
                await this.WriteToLocalFolder();
            };
            async Task setPadWidth(int width)
            {
                this.SettingViewModel.DeviceLayout = new DeviceLayout
                {
                    FallBackType = this.SettingViewModel.DeviceLayout.FallBackType,
                    PhoneMaxWidth = this.SettingViewModel.DeviceLayout.PhoneMaxWidth,
                    PadMaxWidth = width,
                };
                await this.WriteToLocalFolder();
            };


            this.AdaptiveGrid.ScrollModeChanged += (s, mode) =>
            {
                this.ScrollViewer.HorizontalScrollMode = mode;
                this.ScrollViewer.VerticalScrollMode = mode;
            };
            this.AdaptiveGrid.PhoneWidthChanged += async (s, value) => await setPhoneWidth(value);
            this.AdaptiveGrid.PadWidthChanged += async (s, value) => await setPadWidth(value);
            this.AdaptiveResetButton.Click += async (s, e) =>
            {
                //Setting
                this.SettingViewModel.DeviceLayout = DeviceLayout.Default;
                await this.WriteToLocalFolder();//Write

                //Adaptive
                this.NavigatedLayoutAdaptive(DeviceLayout.Default);
            };
        }
        private void NavigatedLayoutAdaptive(DeviceLayout deviceLayout)
        {
            this.AdaptiveGrid.PhoneWidth = deviceLayout.PhoneMaxWidth;
            this.AdaptiveGrid.PadWidth = deviceLayout.PadMaxWidth;
            this.AdaptiveGrid.SetWidth();
        }


        private async Task WriteToLocalFolder()
        {
            await FileUtil.SaveSettingFile(new Setting
            {
                Theme = this.SettingViewModel.Theme,
                DeviceLayout = this.SettingViewModel.DeviceLayout
            });
        }

    }
}