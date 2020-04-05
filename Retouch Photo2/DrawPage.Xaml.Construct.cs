using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using System.Linq;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //ViewModel
        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();
        }
        //KeyboardViewModel
        private void ConstructKeyboardViewModel()
        {
            //Move
            if (this.KeyboardViewModel.Move == null)
            {
                this.KeyboardViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }

            //FullScreen
            if (this.KeyboardViewModel.FullScreenChanged == null)
            {
                this.KeyboardViewModel.FullScreenChanged += (isFullScreen) =>
                {
                    this.IsFullScreen = isFullScreen;
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                this.ViewModel.CanvasTransformer.Width = (int)size.Width;
                this.ViewModel.CanvasTransformer.Height = (int)size.Height;

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        #region Tool


        MoreToolButton MoreToolButton = new MoreToolButton();
        UIElementCollection TooLeft => this.DrawLayout.LeftPaneChildren;

        ToolButtonType _tempToolButtonType;

        private void ConstructTool(ITool tool)
        {
            ToolButtonType type;
            UIElement button = null;
            
            if (tool == null)
            {
                type = this._tempToolButtonType;
                button = new RectangleSeparator();
            }
            else
            {
                type = tool.Button.Type;
                button = tool.Button.Self;

                this._tempToolButtonType = tool.Button.Type;
                tool.Button.Self.Tapped += (s, e) =>
                {
                    this.TipViewModel.ToolGroupType(tool.Type);

                    this.TipViewModel.Tool = tool;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }

            switch (type)
            {
                case ToolButtonType.None:
                    this.TooLeft.Add(button);
                    break;
                case ToolButtonType.Second:
                    this.MoreToolButton.Add(button);
                    break;
            }
        }


        private void ToolFirst()
        {
            ITool tool = this.TipViewModel.Tools.FirstOrDefault();
            if (tool != null)
            {
                //Group
                tool.Button.IsSelected = true;
                tool.Page.IsSelected = true;

                //Changed
                this.TipViewModel.Tool = tool;
                this.DrawLayout.LeftIcon = tool.Icon;

                this.FootPageControl.Content = tool.Page.Self;//FootPage
            }
        }


        #endregion


        #region Menu

        UIElementCollection MennuHead => this.DrawLayout.HeadRightChildren;

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
                    this.MennuHead.Add(menu.Button.Self);
                    break;
                case MenuButtonType.LayersControlIndicator:
                    this.LayersControl.IndicatorBorder.Child = menu.Button.Self;
                    break;
            }
        }

        private void MenusHideAndCrop(bool isCrop)
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                switch (menu.State)
                {
                    case MenuState.FlyoutShow:
                        menu.State = MenuState.FlyoutHide;
                        break;
                    case MenuState.OverlayExpanded:
                    case MenuState.OverlayNotExpanded:
                        if (isCrop)
                        {
                            Point postion = MenuHelper.GetOverlayPostion(menu.Layout.Self);
                            Point postion2 = MenuHelper.GetBoundPostion(postion, menu.Layout.Self);
                            MenuHelper.SetOverlayPostion(menu.Layout.Self, postion2);
                        }
                        break;
                }
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

    }
}