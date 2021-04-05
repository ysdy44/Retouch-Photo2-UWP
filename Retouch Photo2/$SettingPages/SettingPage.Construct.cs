using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2
{
    public sealed partial class SettingPage : Page
    {

        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        //Strings
        public IList<Action<ResourceLoader>> KeyStringsChanged = new List<Action<ResourceLoader>>();
        public IList<Action<ResourceLoader>> CanvasBackgroundChanged = new List<Action<ResourceLoader>>();
        public IList<Action<ResourceLoader>> LanguageStringsChanged = new List<Action<ResourceLoader>>();
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Head.Title = resource.GetString("$SettingPage_Title");
            {
                this.Head.LeftButtonToolTip = resource.GetString("$SettingPage_Back");
                this.Head.RightButtonToolTip = resource.GetString("$SettingPage_About");
            }

            //this.AboutDialog.Title = resource.GetString("$SettingPage_About_Title");
            {
                this.AboutDialog.SecondaryButtonText = resource.GetString("$SettingPage_AboutDialog_Close");
                this.AboutDialog.PrimaryButtonText = resource.GetString("$SettingPage_AboutDialog_Primary");

                this.VersionTextBlock.Text = resource.GetString("$Version");

                this.DocumentationTextBlock.Text = resource.GetString("$SettingPage_Documentation");
                string documentationLink = resource.GetString("$DocumentationLink");
                this.DocumentationHyperlinkButton.Content = documentationLink;
                this.DocumentationHyperlinkButton.NavigateUri = new Uri(documentationLink);

                this.GithubTextBlock.Text = resource.GetString("$SettingPage_Github");
                string githubLink = resource.GetString("$GithubLink");
                this.GithubHyperlinkButton.Content = githubLink;
                this.GithubHyperlinkButton.NavigateUri = new Uri(githubLink);

                this.FeedbackTextBlock.Text = resource.GetString("$SettingPage_Feedback");
                string feedbackLink = resource.GetString("$FeedbackLink");
                this.FeedbackHyperlinkButton.Content = feedbackLink;
                this.FeedbackHyperlinkButton.NavigateUri = new Uri("mailto:" + feedbackLink);
            }

            this.ThemeTextBlock.Text = resource.GetString("$SettingPage_Theme");
            {
                this.LightRadioButton.Content = resource.GetString("$SettingPage_Theme_Light");
                this.DarkRadioButton.Content = resource.GetString("$SettingPage_Theme_Dark");
                this.UseSystemRadioButton.Content = resource.GetString("$SettingPage_Theme_UseSystem");
            }

            this.DeviceLayoutTextBlock.Text = resource.GetString("$SettingPage_DeviceLayout");
            {
                this.PhoneButton.Content = this.AdaptiveWidthGrid.PhoneText = resource.GetString("$SettingPage_DeviceLayout_Phone");
                this.PadButton.Content = this.AdaptiveWidthGrid.PadText = resource.GetString("$SettingPage_DeviceLayout_Pad");
                this.PCButton.Content = this.AdaptiveWidthGrid.PCText = resource.GetString("$SettingPage_DeviceLayout_PC");
                this.AdaptiveButton.Content = resource.GetString("$SettingPage_DeviceLayout_Adaptive");
            }

            this.AdaptiveWidthTextBlock.Text = resource.GetString("$SettingPage_DeviceLayout_AdaptiveWidth");
            this.ResetAdaptiveWidthButton.Content = resource.GetString("$SettingPage_DeviceLayout_ResetAdaptiveWidth");

            this.CanvasBackgroundTextBlock.Text = resource.GetString("$SettingPage_CanvasBackground");
            foreach (var item in this.CanvasBackgroundChanged) item(resource);

            this.LayersHeightTextBlock.Text = resource.GetString("$SettingPage_LayersHeight");
            this.LayersHeightTipTextBlock.Text = resource.GetString("$SettingPage_LayersHeightTip");

            this.MenuTypeTextBlock.Text = resource.GetString("$SettingPage_MenuType");
            {
                foreach (UIElement child in this.MenusStackPanel.Children)
                {
                    if (child is Border border && border.Child is CheckBox check)
                    {
                        string type = check.Name;
                        check.Content = resource.GetString($"Menus_{type}");
                    }
                }
            }
            this.MenuTypeTipTextBlock.Text = resource.GetString("$SettingPage_MenuTypeTip");

            this.KeyTextBlock.Text = resource.GetString("$SettingPage_Key");
            foreach (var item in this.KeyStringsChanged) item(resource);

            this.LanguageTextBlock.Text = resource.GetString("$SettingPage_Language");
            foreach (var item in this.LanguageStringsChanged) item(resource);
            this.LanguageTipTextBlock.Text = resource.GetString("$SettingPage_LanguageTip");

            this.LocalFolderTextBlock.Text = resource.GetString("$SettingPage_LocalFolder");
            this.OpenTextBlock.Text = resource.GetString("$SettingPage_LocalFolder_Open");
        }


        //About
        private void ConstructAbout()
        {
            this.AboutDialog.SecondaryButtonClick += (s, e) => this.AboutDialog.Hide();
            this.AboutDialog.PrimaryButtonClick += (s, e) => this.AboutDialog.Hide();

            int about = 3;
            this.AboutImage.DoubleTapped += (s, e) => this.AboutStoryboard.Begin();//Storyboard
            this.AboutStoryboard.Completed += (s, e) =>
            {
                about--;
                if (about <= 0)
                {
                    about = 3;
                    this.Frame.Navigate(typeof(DebugPage));//Navigate
                }
            };
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

            //Type
            {
                DeviceLayoutType type = deviceLayout.FallBackType;
                bool isAdaptive = deviceLayout.IsAdaptive;

                this.PhoneButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Phone);
                this.PadButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.Pad);
                this.PCButton.IsChecked = (isAdaptive == false && type == DeviceLayoutType.PC);
                this.AdaptiveButton.IsChecked = (isAdaptive);

                this.PhoneButton.Click += async (s, e) => await this.SetDeviceLayoutType(DeviceLayoutType.Phone, false);
                this.PadButton.Click += async (s, e) => await this.SetDeviceLayoutType(DeviceLayoutType.Pad, false);
                this.PCButton.Click += async (s, e) => await this.SetDeviceLayoutType(DeviceLayoutType.PC, false);
                this.AdaptiveButton.Click += async (s, e) => await this.SetDeviceLayoutType(DeviceLayoutType.PC, true);
            }

            //Adaptive
            {
                int phone = deviceLayout.PhoneMaxWidth;
                int pad = deviceLayout.PadMaxWidth;

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
            }


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


        //CanvasBackground        
        private void ConstructCanvasBackground()
        {
            //CanvasBackgrounds
            byte? cannnel = this.SettingViewModel.Setting.CanvasBaclground;

            //UIElementCollection 
            this.CanvasBackgroundChanged.Clear();
            this.CanvasBackgroundStackPanel.Children.Clear();
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(0, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(46, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(92, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(138, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(177, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(216, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(255, cannnel));
            this.CanvasBackgroundStackPanel.Children.Add(constructLayersHeightButton(null, cannnel));

            //Construct
            RadioButton constructLayersHeightButton(byte? value, byte? groupValue)
            {
                RadioButton radioButton = new RadioButton
                {
                    GroupName = "CanvasBackground",
                    Style = this.RadioButtonStyle,
                    IsChecked = groupValue == value,
                };

                if (value is byte cannel)
                {
                    radioButton.Content = value;
                    radioButton.Tag = new ColorEllipse
                    {
                        Color = Windows.UI.Color.FromArgb(255, cannel, cannel, cannel)
                    };
                }
                else
                {
                    //Strings
                    this.CanvasBackgroundChanged.Add((resource) => radioButton.Content = resource.GetString("$SettingPage_CanvasBackground_None"));

                    radioButton.Tag = new ColorEllipse
                    {
                        Color = Colors.Transparent
                    };
                }

                radioButton.Click += async (s, e) => await this.SetCanvasBackground(value);
                return radioButton;
            }
        }


        //LayersHeight        
        private void ConstructLayersHeight()
        {
            //LayersHeights
            int layersHeight = this.SettingViewModel.Setting.LayersHeight;

            //UIElementCollection 
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(30, layersHeight));
            this.LayersHeightStackPanel.Children.Add(new Rectangle { Style = this.RectangleStyle });
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(40, layersHeight));
            this.LayersHeightStackPanel.Children.Add(new Rectangle { Style = this.RectangleStyle });
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(50, layersHeight));
            this.LayersHeightStackPanel.Children.Add(new Rectangle { Style = this.RectangleStyle });
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(60, layersHeight));
            this.LayersHeightStackPanel.Children.Add(new Rectangle { Style = this.RectangleStyle });
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(70, layersHeight));
            this.LayersHeightStackPanel.Children.Add(new Rectangle { Style = this.RectangleStyle });
            this.LayersHeightStackPanel.Children.Add(constructLayersHeightButton(80, layersHeight));

            //Construct
            RadioButton constructLayersHeightButton(int value, int groupValue)
            {
                RadioButton radioButton = new RadioButton
                {
                    GroupName = "LayersHeight",
                    Style = this.LayerHeightRadioButtonStyle,
                    IsChecked = groupValue == value,
                    Content = new LayerControl(value, $"{value}")
                    {
                        IsHitTestVisible = false
                    },
                };

                radioButton.Click += async (s, e) => await this.SetLayersHeight(value);
                return radioButton;
            }
        }


        //MenuType
        private void ConstructMenuType()
        {
            //UIElementCollection 
            foreach (UIElement child in this.MenusStackPanel.Children)
            {
                if (child is Border border && border.Child is CheckBox check)
                {
                    string type = check.Name;
                    bool isContains = this.SettingViewModel.Setting.MenuTypes.Contains(type);

                    check.IsChecked = isContains;
                    check.Checked += async (s, e) => await this.AddMenu(type);
                    check.Unchecked += async (s, e) => await this.RemoveMenu(type);
                }
            }
        }


        //Key
        private void ConstructKey()
        {
            //Style
            int index = 0;
            Style getStyle2() => ((index++) % 2 == 0) ? this.KeyContentControlBackgroundStyle : this.KeyContentControlStyle;

            //Keys
            IList<KeyboardAccelerator2> keys = this.SettingViewModel.KeyboardAccelerators;

            //UIElementCollection 
            this.KeyStringsChanged.Clear();
            this.Key00StackPanel.Children.Clear();
            this.Key01StackPanel.Children.Clear();
            this.Key02StackPanel.Children.Clear();
            this.Key03StackPanel.Children.Clear();

            this.Key00StackPanel.Children.Add(constructKeyContentControl("Shift", "Tools_MoreTransform_Ratio"));
            this.Key00StackPanel.Children.Add(constructKeyContentControl("Shift", "Tools_MoreCreate_Square"));
            this.Key00StackPanel.Children.Add(constructKeyContentControl("Ctrl", "Tools_MoreCreate_Center"));
            this.Key00StackPanel.Children.Add(constructKeyContentControl("Space", "Menus_Transformer_StepFrequency"));
            this.Key00StackPanel.Children.Add(constructKeyContentControl("Space", "$SettingPage_Key_Rotate"));

            foreach (KeyboardAccelerator2 key in keys)
            {
                switch (key.Group)
                {
                    case 1: this.Key01StackPanel.Children.Add(constructKeyContentControl(key.ToString(), key.TitleResource)); break;
                    case 2: this.Key02StackPanel.Children.Add(constructKeyContentControl(key.ToString(), key.TitleResource)); break;
                    case 3: this.Key03StackPanel.Children.Add(constructKeyContentControl(key.ToString(), key.TitleResource)); break;
                }
            }

            //Construct
            ContentControl constructKeyContentControl(string key, string titleResource)
            {
                ContentControl contentControl = new ContentControl    
                {         
                    Tag = key.ToString(),        
                    Style = getStyle2()            
                };

                //Strings
                this.KeyStringsChanged.Add((resource) => contentControl.Content = resource.GetString(titleResource));

                return contentControl;
            }
        }


        //Language
        private void ConstructLanguage()
        {
            //Style
            int index = 0;
            Style getStyle() => ((index++) % 2 == 0) ? this.MenuBorderStyle2 : this.MenuBorderStyle1;

            //Languages
            string groupLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            List<string> languages = new List<string>(ApplicationLanguages.ManifestLanguages);
            languages.Sort();

            //UIElementCollection 
            this.LanguageStringsChanged.Clear();
            this.LanguageStackPanel.Children.Clear();
            this.LanguageStackPanel.Children.Add(new Border
            {
                Child = constructLanguageRadioButton(string.Empty),
                Style = getStyle()
            });
            foreach (string language in languages)
            {
                this.LanguageStackPanel.Children.Add(new Border
                {
                    Child = constructLanguageRadioButton(language),
                    Style = getStyle()
                });
            }

            //Construct
            RadioButton constructLanguageRadioButton(string language)
            {
                RadioButton radioButton = new RadioButton
                {
                    GroupName = "Language",
                    IsChecked = groupLanguage == language,
                };

                //Use system settin
                if (string.IsNullOrEmpty(language))
                {
                    //Strings
                    this.LanguageStringsChanged.Add((resource) => radioButton.Content = resource.GetString("$SettingPage_Language_UseSystemSetting"));
                }
                else
                {
                    ToolTipService.SetToolTip(radioButton, new ToolTip
                    {
                        Content = language,
                        Style = this.ToolTipStyle
                    });

                    radioButton.ContentTemplate = this.LanguageTemplate;
                    radioButton.Content = new CultureInfo(language);
                }

                radioButton.Checked += (s, e) =>
                {
                    if (ApplicationLanguages.PrimaryLanguageOverride == language) return;
                    ApplicationLanguages.PrimaryLanguageOverride = language;

                    if (string.IsNullOrEmpty(language) == false) this.Language = language;
                    this.ConstructFlowDirection();
                    this.ConstructStrings();
                };

                return radioButton;
            }

        }

    }
}