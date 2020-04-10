using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //Tool & Menu
        private void ConstructToolAndMenu()
        {
            //Tool
            foreach (ITool tool in this.TipViewModel.Tools)
            {
                this.ConstructTool(tool);
            }
            UIElement moreButton = this.ToolLeft.First();
            this.ToolLeft.Remove(moreButton);
            this.ToolLeft.Add(moreButton);
            this.ToolFirst();
            

            //Menu
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenu(menu);
            }
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHide();
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop();
        }


        /// <summary> what is last? </summary>
        ToolButtonType _lockToolButtonType;
        
        /// <summary> Left panel of Tool. </summary>
        UIElementCollection ToolLeft => this.DrawLayout.LeftPanelChildren;
        /// <summary> Left more panel of Tool. </summary>
        UIElementCollection ToolLeftMore => this.DrawLayout.LeftMorePanelChildren;

        //Tool
        private void ConstructTool(ITool tool)
        {
            ToolButtonType type;
            UIElement button = null;

            if (tool == null)
            {
                type = this._lockToolButtonType;
                button = new RectangleSeparator();
            }
            else
            {
                type = tool.Button.Type;
                button = tool.Button.Self;

                this._lockToolButtonType = tool.Button.Type;
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
                    this.ToolLeft.Add(button);
                    break;
                case ToolButtonType.Second:
                    this.ToolLeftMore.Add(button);
                    break;
            }
        }

        #region Tool


        /// <summary>
        /// Select the first Tool by default. 
        /// </summary>
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

        
        /// <summary> Head panel of Menu. </summary>
        UIElementCollection MenuHead => this.DrawLayout.HeadRightChildren;
        
        //Menu
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
                    this.MenuHead.Add(menu.Button.Self);
                    break;
                case MenuButtonType.LayersControlIndicator:
                    this.LayersControl.IndicatorBorder.Child = menu.Button.Self;
                    break;
            }
        }
        
        #region Menu


        /// <summary>
        /// Hide all Menus.
        /// </summary>
        private void MenusHide() => this._menusHideAndCrop(false);

        /// <summary>
        /// Hide all Menus, and crop the limiting bound.
        /// </summary>
        private void MenusHideAndCrop() => this._menusHideAndCrop(true);

        private void _menusHideAndCrop(bool isCrop)
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


        /// <summary>
        /// Disable all menus, except the current menu.
        /// </summary>
        /// <param name="currentMenu"> The current menu. </param>
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
        /// <summary>
        /// Enable all menus.
        /// </summary>
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