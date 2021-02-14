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
    public sealed partial class SettingPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.TitleTextBlock.Text = resource.GetString("$SettingPage_Title");
            {
                this.BackToolTip.Content = resource.GetString("$SettingPage_Back");
                this.AboutToolTip.Content = resource.GetString("$SettingPage_About");
            }

            this.ThemeTextBlock.Text = resource.GetString("$SettingPage_Theme");
            {
                this.LightRadioButton.Content = resource.GetString("$SettingPage_Theme_Light");
                this.DarkRadioButton.Content = resource.GetString("$SettingPage_Theme_Dark");
                this.UseSystemRadioButton.Content = resource.GetString("$SettingPage_Theme_UseSystem");
            }

            this.DeviceLayoutTextBlock.Text = resource.GetString("$SettingPage_DeviceLayout");
            {
                this.PhoneButton.Content = resource.GetString("$SettingPage_DeviceLayout_Phone");
                this.PadButton.Content = resource.GetString("$SettingPage_DeviceLayout_Pad");
                this.PCButton.Content = resource.GetString("$SettingPage_DeviceLayout_PC");
                this.AdaptiveButton.Content = resource.GetString("$SettingPage_DeviceLayout_Adaptive");
            }

            this.AdaptiveWidthTextBlock.Text = resource.GetString("$SettingPage_DeviceLayout_AdaptiveWidth");
            this.ResetAdaptiveWidthButton.Content = resource.GetString("$SettingPage_DeviceLayout_ResetAdaptiveWidth");

            this.LayersHeightTextBlock.Text = resource.GetString("$SettingPage_LayersHeight");
            this.LayersHeightTipTextBlock.Text = resource.GetString("$SettingPage_LayersHeightTip");

            this.KeyTextBlock.Text = resource.GetString("$SettingPage_Key");
            this.Key01Border.Child = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Tag = "Shift",
                        Content = resource.GetString("Tools_MoreTransform_Ratio"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Shift",
                        Content = resource.GetString("Tools_MoreCreate_Square"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl",
                        Content = resource.GetString("Tools_MoreCreate_Center"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Space",
                        Content = resource.GetString("Menus_Transformer_StepFrequency"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Esc",
                        Content = resource.GetString("$DrawPage_FullScreen"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                }
            };

            this.Key02Border.Child = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Tag = "←",
                        Content = resource.GetString("$SettingPage_Key_Move_Left"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "↑",
                        Content = resource.GetString("$SettingPage_Key_Move_Top"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "→",
                        Content = resource.GetString("$SettingPage_Key_Move_Right"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "↓",
                        Content = resource.GetString("$SettingPage_Key_Move_Bottom"),
                        Style = this.KeyContentControlStyle
                    },
                }
            };

            this.Key03Border.Child = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Tag = "Ctrl + X",
                        Content = resource.GetString("Edits_Edit_Cut"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + J",
                        Content = resource.GetString("Edits_Edit_Duplicate"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + C",
                        Content = resource.GetString("Edits_Edit_Copy"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + V",
                        Content = resource.GetString("Edits_Edit_Paste"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Delete",
                        Content = resource.GetString("Edits_Edit_Clear"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + A",
                        Content = resource.GetString("Edits_Select_All"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + D",
                        Content = resource.GetString("Edits_Select_Deselect"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + I",
                        Content = resource.GetString("Edits_Select_Invert"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + G",
                        Content = resource.GetString("Edits_Group_Group"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + U",
                        Content = resource.GetString("Edits_Group_UnGroup"),
                        Style = this.KeyContentControlStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + R",
                        Content = resource.GetString("Edits_Group_Release"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                }
            };

            this.Key04Border.Child = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Tag = "Ctrl + E",
                        Content = resource.GetString("$DrawPage_Export"),
                        Style = this.KeyContentControlBackgroundStyle
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl + Z",
                        Content = resource.GetString("$DrawPage_Undo"),
                        Style = this.KeyContentControlStyle
                    },
                }
            };

            this.MenuTypeTextBlock.Text = resource.GetString("$SettingPage_MenuType");
            this.MenuTypeTipTextBlock.Text = resource.GetString("$SettingPage_MenuTypeTip");

            this.LocalFolderTextBlock.Text = resource.GetString("$SettingPage_LocalFolder");
            this.OpenTextBlock.Text = resource.GetString("$SettingPage_LocalFolder_Open");
        }



        //Theme
        private void ConstructTheme()
        {
            ElementTheme theme = this.SettingViewModel.Setting.Theme;
            this.LightRadioButton.IsChecked = (theme == ElementTheme.Light);
            this.DarkRadioButton.IsChecked = (theme == ElementTheme.Dark);
            this.UseSystemRadioButton.IsChecked = (theme == ElementTheme.Default);
            
            this.LightRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Light);
            this.DarkRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Dark);
            this.UseSystemRadioButton.Click += async (s, e) => await this.SetTheme(ElementTheme.Default);
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
            this.AdaptiveWidthGrid.PhoneWidth = phone;
            this.AdaptiveWidthGrid.PadWidth = pad;
            this.AdaptiveWidthGrid.SetWidth();


            this.AdaptiveWidthGrid.ScrollModeChanged += (s, mode) =>
            {
                this.ScrollViewer.HorizontalScrollMode = mode;
                this.ScrollViewer.VerticalScrollMode = mode;
            };
            this.AdaptiveWidthGrid.PhoneWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PhoneMaxWidth = value;
                this.SettingViewModel.NotifyDeviceLayoutType();
                await this.Write();
            };
            this.AdaptiveWidthGrid.PadWidthChanged += async (s, value) =>
            {
                //Setting
                this.SettingViewModel.Setting.DeviceLayout.PadMaxWidth = value;
                this.SettingViewModel.NotifyDeviceLayoutType();
                await this.Write();
            };


            this.ResetAdaptiveWidthButton.Click += async (s, e) =>
            {
                DeviceLayout default2 = DeviceLayout.Default;
                int phone2 = default2.PhoneMaxWidth;
                int pad2 = default2.PadMaxWidth;

                this.AdaptiveWidthGrid.PhoneWidth = phone2;
                this.AdaptiveWidthGrid.PadWidth = pad2;
                this.AdaptiveWidthGrid.SetWidth();

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
            bool _isParity = false;
            Style getStyle()
            {
                _isParity = !_isParity;
                return _isParity ? this.MenuBorderStyle1 : this.MenuBorderStyle2;
            }


            IList<MenuType> menuTypes = this.SettingViewModel.Setting.MenuTypes;
            CheckBox createMenuCheckBox(IMenu menu)
            {
                bool isContains = menuTypes.Contains(menu.Type);

                CheckBox checkBox = new CheckBox
                {
                    Content = menu.Button.Title,
                    IsChecked = isContains,
                };
                checkBox.Checked += async (s, e) => await this.AddMenu(menu.Type);
                checkBox.Unchecked += async (s, e) => await this.RemoveMenu(menu.Type);

                return checkBox;
            }


            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.MenusStackPanel.Children.Add(new Border
                {
                    Child = createMenuCheckBox(menu),
                    Style = getStyle(),
                });
            }
        }



    }
}
