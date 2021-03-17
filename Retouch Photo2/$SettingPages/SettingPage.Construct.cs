﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Globalization;
using System.Globalization;

namespace Retouch_Photo2
{
    public sealed partial class SettingPage : Page
    {
        int index = 0;
        Style getStyle()
        {
            index++;
            return (index % 2 == 0) ? this.MenuBorderStyle2 : this.MenuBorderStyle1;
        }
        Style getStyle2()
        {
            index++;
            return (index % 2 == 0) ? this.KeyContentControlBackgroundStyle : this.KeyContentControlStyle;
        }


        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
            
            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Head.Title = resource.GetString("$SettingPage_Title");
            {
                this.Head.LeftButtonToolTip = resource.GetString("$SettingPage_Back");
                this.Head.RightButtonToolTip = resource.GetString("$SettingPage_About");
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
            this.Key00Border.Child = new StackPanel
            {
                Children =
                {
                    new ContentControl
                    {
                        Tag = "Shift",
                        Content = resource.GetString("Tools_MoreTransform_Ratio"),
                        Style = this.getStyle2()
                    },
                    new ContentControl
                    {
                        Tag = "Shift",
                        Content = resource.GetString("Tools_MoreCreate_Square"),
                        Style = this.getStyle2()
                    },
                    new ContentControl
                    {
                        Tag = "Ctrl",
                        Content = resource.GetString("Tools_MoreCreate_Center"),
                        Style = this.getStyle2()
                    },
                    new ContentControl
                    {
                        Tag = "Space",
                        Content = resource.GetString("Menus_Transformer_StepFrequency"),
                        Style = this.getStyle2()
                    },
                    new ContentControl
                    {
                        Tag = "Space",
                        Content = resource.GetString("$SettingPage_Key_Rotate"),
                        Style = this.getStyle2()
                    }
                }
            };

            if (this.SettingViewModel.KeyboardAccelerators is IList<KeyboardAccelerator2> keys)
            {
                foreach (var key in from k in keys where k.Group == 1 select k)
                {
                    this.Key01StackPanel.Children.Add(new ContentControl
                    {
                        Tag = key.ToString(),
                        Content = resource.GetString(key.TitleResource),
                        Style = this.getStyle2()
                    });
                }

                foreach (var key in from k in keys where k.Group == 2 select k)
                {
                    this.Key02StackPanel.Children.Add(new ContentControl
                    {
                        Tag = key.ToString(),
                        Content = resource.GetString(key.TitleResource),
                        Style = this.getStyle2()
                    });
                }

                foreach (var key in from k in keys where k.Group == 3 select k)
                {
                    this.Key03StackPanel.Children.Add(new ContentControl
                    {
                        Tag = key.ToString(),
                        Content = resource.GetString(key.TitleResource),
                        Style = this.getStyle2()
                    });
                }
            }


            this.MenuTypeTextBlock.Text = resource.GetString("$SettingPage_MenuType");
            this.MenuTypeTipTextBlock.Text = resource.GetString("$SettingPage_MenuTypeTip");


            this.LanguageTextBlock.Text = resource.GetString("$SettingPage_Language");
            this.LanguageTipTextBlock.Text = resource.GetString("$SettingPage_LanguageTip");
            this.UseSystemSettingRadioButton.Content = resource.GetString("$SettingPage_Language_UseSystemSetting");


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
                DeviceLayout layout = this.SettingViewModel.Setting.DeviceLayout;
                {
                    layout.PhoneMaxWidth = value;
                    DeviceLayoutType type = layout.GetActualType(this.ActualWidth);
                    this.SettingViewModel.DeviceLayoutType = type;
                }
                await this.Save();
            };
            this.AdaptiveWidthGrid.PadWidthChanged += async (s, value) =>
            {
                //Setting
                DeviceLayout layout = this.SettingViewModel.Setting.DeviceLayout;
                {
                    layout.PadMaxWidth = value;
                    DeviceLayoutType type = layout.GetActualType(this.ActualWidth);
                    this.SettingViewModel.DeviceLayoutType = type;
                }
                await this.Save();
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
                DeviceLayout layout = this.SettingViewModel.Setting.DeviceLayout;
                {
                    layout.PhoneMaxWidth = phone2;
                    layout.PadMaxWidth = pad2;
                    DeviceLayoutType type = layout.GetActualType(this.ActualWidth);
                    this.SettingViewModel.DeviceLayoutType = type;
                }
                await this.Save();
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
                IsHitTestVisible = false
            };
            radioButton.Click += async (s, e) => await this.SetHeight(value);
        }



        //MenuType
        private void ConstructMenuType()
        {
            IList<MenuType> menuTypes = this.SettingViewModel.Setting.MenuTypes;

            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.MenusStackPanel.Children.Add(new Border
                {
                    Child = this.ConstructMenuTypeCheckBox(menu, menuTypes),
                    Style = this.getStyle()
                });
            }
        }
        private CheckBox ConstructMenuTypeCheckBox(IMenu menu, IList<MenuType> menuTypes)
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



        //Language
        private void ConstructLanguage()
        {
            string groupLanguage = this.SettingViewModel.Setting.Language;

            this.UseSystemSettingRadioButton.GroupName = "Language";
            this.UseSystemSettingRadioButton.IsChecked = string.IsNullOrEmpty(groupLanguage);

            List<string> languages = new List<string>(ApplicationLanguages.ManifestLanguages);
            languages.Sort();
            foreach (string language in languages)
            {
                CultureInfo culture = new CultureInfo(language);
                this.ConstructLanguageRadioButton(language, new RadioButton
                {
                    GroupName = "Language",
                    IsChecked = groupLanguage == language,
                    Content = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children=
                        {
                            new TextBlock
                            {
                                Text = culture.NativeName,
                            },
                            new TextBlock
                            {
                                Text = culture.DisplayName,
                                Margin = new Thickness(12, 0, 0, 0),
                                Opacity = 0.5d
                            }
                        }
                    }
                });
            }
        }
        private void ConstructLanguageRadioButton(string language, RadioButton radioButton)
        {
            radioButton.Checked += async (s, e) => await this.SetLanguage(language);

            ToolTipService.SetToolTip(radioButton, new ToolTip
            {
                Content = language,
                Style = this.ToolTipStyle
            });
            this.LanguageStackPanel.Children.Add(new Border
            {
                Child = radioButton,
                Style = this.getStyle()
            });
        }


    }
}
