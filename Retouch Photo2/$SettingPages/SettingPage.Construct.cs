using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Represents a page used to set options.
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("/$SettingPage/Title");
            this.BackToolTip.Content = resource.GetString("/$SettingPage/Page_Back");
            this.AboutToolTip.Content = resource.GetString("/$SettingPage/Page_About");

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

            this.LayersHeightTextBlock.Text = resource.GetString("/$SettingPage/LayersHeight");
            this.LayersHeightTipTextBlock.Text = resource.GetString("/$SettingPage/LayersHeightTip");


            this.KeyTextBlock.Text = resource.GetString("/$SettingPage/Key");

            this.IsCenterControl.Tag = "Ctrl";
            this.IsCenterControl.Content = resource.GetString("/$SettingPage/Key_IsCenter");
            this.IsRatioControl.Tag = "Shift";
            this.IsRatioControl.Content = resource.GetString("/$SettingPage/Key_IsRatio");
            this.IsSquareControl.Tag = "Shift";
            this.IsSquareControl.Content = resource.GetString("/$SettingPage/Key_IsSquare");
            this.IsStepFrequencyControl.Tag = "Space";
            this.IsStepFrequencyControl.Content = resource.GetString("/$SettingPage/Key_IsStepFrequency");
            this.IsFullScreenControl.Tag = "Esc";
            this.IsFullScreenControl.Content = resource.GetString("/$SettingPage/Key_IsFullScreen");

            this.LeftControl.Tag = "←";
            this.LeftControl.Content = resource.GetString("/$SettingPage/Key_MoveLeft ");
            this.UpControl.Tag = "↑";
            this.UpControl.Content = resource.GetString("/$SettingPage/Key_MoveUp ");
            this.RightControl.Tag = "→";
            this.RightControl.Content = resource.GetString("/$SettingPage/Key_MoveRight ");
            this.DownControl.Tag = "↓";
            this.DownControl.Content = resource.GetString("/$SettingPage/Key_MoveDown ");

            this.CutControl.Tag = "Ctrl + X";
            this.CutControl.Content = resource.GetString("/$SettingPage/Key_EditCut");
            this.DuplicateControl.Tag = "Ctrl + J";
            this.DuplicateControl.Content = resource.GetString("/$SettingPage/Key_EditDuplicate");
            this.CopyControl.Tag = "Ctrl + C";
            this.CopyControl.Content = resource.GetString("/$SettingPage/Key_EditCopy");
            this.PasteControl.Tag = "Ctrl + V";
            this.PasteControl.Content = resource.GetString("/$SettingPage/Key_EditPaste");
            this.ClearControl.Tag = "Delete";
            this.ClearControl.Content = resource.GetString("/$SettingPage/Key_EditClear");
            this.AllControl.Tag = "Ctrl + A";
            this.AllControl.Content = resource.GetString("/$SettingPage/Key_EditAll");
            this.DeselectControl.Tag = "Ctrl + D";
            this.DeselectControl.Content = resource.GetString("/$SettingPage/Key_EditDeselect");
            this.InvertControl.Tag = "Ctrl + I";
            this.InvertControl.Content = resource.GetString("/$SettingPage/Key_EditInvert");
            this.GroupControl.Tag = "Ctrl + G";
            this.GroupControl.Content = resource.GetString("/$SettingPage/Key_EditGroup");
            this.UnGroupControl.Tag = "Ctrl + U";
            this.UnGroupControl.Content = resource.GetString("/$SettingPage/Key_EditUnGroup");
            this.ReleaseControl.Tag = "Ctrl + R";
            this.ReleaseControl.Content = resource.GetString("/$SettingPage/Key_EditRelease");
            this.UndoControl.Tag = "Ctrl + Z";
            this.UndoControl.Content = resource.GetString("/$SettingPage/Key_EditUndo");
            

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
            
            this.LightRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Light);
            this.DarkRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Dark);
            this.DefaultRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Default);
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
            this.IsAdaptive = isAdaptive;
            this.PhoneButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Phone);
            this.PadButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Pad);
            this.PCButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.PC);
            this.AdaptiveButton.IsChecked = (isAdaptive);
                       
            this.PhoneButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.Phone, false);
            this.PadButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.Pad, false);
            this.PCButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.PC, false);
            this.AdaptiveButton.Click += async (s, e) => await this.SetType(DeviceLayoutType.PC, true);
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
                this.SettingViewModel.NotifyDeviceLayoutType();
                await this.Write();
            };
            this.AdaptiveGrid.PadWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = value;
                this.SettingViewModel.NotifyDeviceLayoutType();
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
                this.SettingViewModel.NotifyDeviceLayoutType();
                await this.Write();//Write
            };
        }



        //LayersHeight        
        private void ConstructLayersHeight()
        {
            int height = this.SettingViewModel.Setting.LayersHeight;

            this.ConstructLayersHeightButton(this.Height30Button, 30, height);
            this.ConstructLayersHeightButton(this.Height40Button, 40, height);
            this.ConstructLayersHeightButton(this.Height50Button, 50, height);
            this.ConstructLayersHeightButton(this.Height60Button, 60, height);
            this.ConstructLayersHeightButton(this.Height70Button, 70, height);
            this.ConstructLayersHeightButton(this.Height80Button, 80, height);
        }

        private void ConstructLayersHeightButton(RadioButton radioButton, int value, int groupValue)
        {
            string type = this.LayersHeightTextBlock.Text;
            radioButton.IsChecked = groupValue == value;
            radioButton.Content = new LayerControl(value, $"{type} {value}")
            {
                IsEnabled = false
            };  
            radioButton.Click += async (s, e) => await this.SetHeight(value);
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
                    Content = menu.Button.Title,
                    IsChecked = isVisible,
                    FontWeight = FontWeights.Medium
                };
                checkBox.Checked += async (s, e) => await this.AddMenu(menu.Type);
                checkBox.Unchecked += async (s, e) => await this.RemoveMenu(menu.Type);

                isParity = !isParity;
                this.MenusStackPanel.Children.Add(new Border
                {
                    Child = checkBox,
                    Style = this.MenuBorderStyle,
                });
            }
        }



    }
}
