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
            this.ToolFirst();


            this.TipViewModel.Menus.Add(this.SelectionMenu);
            this.TipViewModel.Menus.Add(this.OperateMenu);
            this.TipViewModel.Menus.Add(this.AdjustmentMenu);
            this.TipViewModel.Menus.Add(this.EffectMenu);
            this.TipViewModel.Menus.Add(this.TransformerMenu);
            this.TipViewModel.Menus.Add(this.CharacterMenu);
            this.TipViewModel.Menus.Add(this.ColorMenu);
            this.TipViewModel.Menus.Add(this.LayerMenu);
            //Menu
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenu(menu);
            }
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHide();
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop();
        }


        /// <summary> Left panel of Tool. </summary>
        UIElementCollection ToolLeft => this.DrawLayout.LeftPanelChildren;
        /// <summary> Left more button's flyout panel's children. </summary>
        UIElementCollection ToolLeftMore = null;

        //Tool
        private void ConstructTool(ITool tool)
        {
            if (tool == null)
            {
                if (this.ToolLeftMore == null)
                    this.ToolLeft.Add(new ComboBoxSeparator());
                else
                    this.ToolLeftMore.Add(new ComboBoxSeparator());

                return;
            }
            else if (tool.Type == ToolType.None)
            {
                return;
            }
            else if (tool.Type == ToolType.More)
            {
                if (tool.Button is ToolMoreButton moreButton)
                {
                    this.ToolLeft.Add(moreButton);
                    this.ToolLeftMore = moreButton.Children;
                }
                return;
            }
            else
            {
                if (tool.Button is FrameworkElement element)
                {
                    if (this.ToolLeftMore == null)
                        this.ToolLeft.Add(element);
                    else
                        this.ToolLeftMore.Add(element);

                    element.Tapped += (s, e) =>
                    {
                        this.TipViewModel.ToolGroupType(tool.Type);

                        this.TipViewModel.Tool = tool;

                        this.ViewModel.Invalidate();//Invalidate
                    };
                }
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
                this.TipViewModel.ToolGroupType(tool.Type);

                this.TipViewModel.Tool = tool;
            }
        }


        #endregion


        /// <summary> Head panel of Menu. </summary>
        UIElementCollection MenuHead => this.DrawLayout.HeadRightChildren;

        //Menu
        public void ConstructMenu(IMenu menu)
        {
            if (menu == null) return;

            // this.OverlayCanvas.Children.Add(menu.Layout);

            menu.Move += () =>
            {
                //Move to top
                int index = this.OverlayCanvas.Children.IndexOf(menu.Layout);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1);
            };
            menu.Opened += () => this.MenusDisable(menu);//Menus is disable
            menu.Closed += () => this.MenusEnable();//Menus is enable

            //MenuButton
            if (menu.Type == MenuType.Layer)
            {
                this.LayersControl.IndicatorBorder.Child = menu.Button;
            }
            else
            {
                this.MenuHead.Add(menu.Button);
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
                        menu.State = MenuState.Hide;
                        break;
                    case MenuState.Overlay:
                    case MenuState.OverlayNotExpanded:
                        if (isCrop)
                        {
                            Point postion = MenuHelper.GetOverlayPostion(menu.Layout);
                            Point postion2 = MenuHelper.GetBoundPostion(postion, menu.Layout);
                            MenuHelper.SetOverlayPostion(menu.Layout, postion2);
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