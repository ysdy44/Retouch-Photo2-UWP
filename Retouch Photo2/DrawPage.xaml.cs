using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Converter
        public Visibility BoolToVisibilityConverter(bool isChecked) => isChecked ? Visibility.Visible : Visibility.Collapsed;


        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();

            //Appbar
            this.DrawLayout.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (s, e) => this.Frame.GoBack();

            this.ThemeButton.Loaded += (s, e) =>
            {
                ElementTheme theme = this.SettingViewModel.ElementTheme;
                this.ThemeControl.Theme = theme;
            };
            this.ThemeButton.Tapped += (s, e) =>
            {
                //Trigger switching theme.
                ElementTheme theme = this.ThemeControl.Theme;
                theme = (theme == ElementTheme.Dark) ? ElementTheme.Light : ElementTheme.Dark;

                this.RequestedTheme = theme;
                this.ThemeControl.Theme = theme;
                ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);
            };


            //FullScreen
            this.UnFullScreenButton.Tapped += (s, e) => this.DrawLayout.IsFullScreen = !DrawLayout.IsFullScreen;
            this.FullScreenButton.Tapped += (s, e) => this.DrawLayout.IsFullScreen = !this.DrawLayout.IsFullScreen;
     
            
            //Layers
            this.LayersControl.WidthButton.Tapped += (s, e) => this.DrawLayout.PadChangeLayersWidth();


            //Tool
            foreach (ITool tool in this.TipViewModel.Tools)
            {
                this.ConstructTool(tool);
            }
            this.TooLeft.Add(this.MoreToolButton);

            //Menu
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenu(menu);
            }
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHide();
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop();
        }


        #region Tool


        MoreToolButton MoreToolButton = new MoreToolButton();
        UIElementCollection MoreTool => this.MoreToolButton.StackPanel.Children;

        UIElementCollection TooLeft => this.DrawLayout.LeftPaneChildren;

        ToolButtonType _tempToolButtonType;
 
        private void ConstructTool(ITool tool)
        {
            if (tool == null)
            {
                Rectangle rectangle = new Rectangle
                {
                    Fill = new SolidColorBrush(Windows.UI.Colors.Gray),
                    Height = 1,
                    Opacity = 0.2,
                    Margin = new Thickness(4, 0, 4, 0),
                };

                switch (this._tempToolButtonType)
                {
                    case ToolButtonType.None:
                        this.TooLeft.Add(rectangle);
                        break;
                    case ToolButtonType.Second:
                        this.MoreTool.Add(rectangle);
                        break;
                }
                return;
            }
            IToolButton button = tool.Button;

            if (button!=null)
            {
                button.Self.Tapped += (s, e) =>
                {                    
                    this.ToolGroupType(tool.Type);//Tools
                    this.TipViewModel.Tool = tool;//Tool

                    //DrawLayout
                    this.DrawLayout.LeftIcon = tool.Icon;
                    this.DrawLayout.FootPage = tool.Page.Self;
                };

                this._tempToolButtonType = button.Type;
                switch (button.Type)
                {
                    case ToolButtonType.None:
                        this.TooLeft.Add(button.Self);
                        break;
                    case ToolButtonType.Second:
                        this.MoreTool.Add(button.Self);
                        break;
                }
            }
        }

        private void ToolGroupType(ToolType groupType)
        {
            foreach (ITool tool in this.TipViewModel.Tools)
            {
                if (tool != null)
                {
                    bool isSelected = (tool.Type == groupType);

                    tool.Button.IsSelected = isSelected;
                    tool.Page.IsSelected = isSelected;
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }


        #endregion

        #region Menu


        public void ConstructMenu(IMenu menu)
        {
            if (menu == null) return;

            this.OverlayCanvas.Children.Add(menu.Layout.Self);

            menu.Move += () =>
            {
                //Move to top
                int index = this.OverlayCanvas.Children.IndexOf(menu.Layout.Self);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1);
            };
            menu.Opened += () => this.MenusDisable(menu);//Menus is disable
            menu.Closed += () => this.MenusEnable();//Menus is enable

            //MenuButton
            switch (menu.Button.Type)
            {
                case MenuButtonType.None:
                    this.DrawLayout.HeadRightChildren.Add(menu.Button.Self);
                    break;
                case MenuButtonType.LayersControlIndicator:
                    this.LayersControl.IndicatorBorder.Child = menu.Button.Self;
                    break;
            }
        }

        private void MenusHide()
        {
            foreach (IMenu other in this.TipViewModel.Menus)
            {
                other.Hide();
            }
            this.OverlayCanvas.Background = null;
        }
        private void MenusHideAndCrop()
        {
            foreach (IMenu other in this.TipViewModel.Menus)
            {
                other.Hide();
                other.Crop();
            }
            this.OverlayCanvas.Background = null;
        }

        private void MenusDisable(IMenu currentMenu)
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                if (menu.Type != currentMenu.Type)
                {
                    menu.IsHitTestVisible = false;
                }
            }
            this.OverlayCanvas.Background = new SolidColorBrush(Colors.Transparent);
        }
        private void MenusEnable()
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                menu.IsHitTestVisible = true;
            }
            this.OverlayCanvas.Background = null;
        }


        #endregion

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.RequestedTheme = theme;
            this.ThemeControl.Theme = theme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);

            //Layout
            this.DrawLayout.VisualStateDeviceType = this.SettingViewModel.LayoutDeviceType;
            this.DrawLayout.VisualStatePhoneMaxWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.DrawLayout.VisualStatePadMaxWidth = this.SettingViewModel.LayoutPadMaxWidth;


            return;
            if (e.Parameter is Project project)
            {
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }

             //   this.Loaded += (sender, e2) =>
                //{

            this.LoadingControl.Visibility = Visibility.Visible;//Loading
            this.ViewModel.LoadFromProject(project);//Project
            this.LoadingControl.Visibility = Visibility.Collapsed;//Loading   

            this.ViewModel.Invalidate();
               // };
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
    }
}