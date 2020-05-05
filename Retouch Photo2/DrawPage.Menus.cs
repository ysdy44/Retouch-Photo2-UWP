using Retouch_Photo2.Menus;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //Menu
        public void ConstructMenus()
        {
            //Menu
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHide();
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop();
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                if (menu == null) return;

                this.ConstructMenu(menu);

                //MenuButton
                if (menu.Type == MenuType.Layer)
                {
                    this.LayersControl.IndicatorBorder.Child = menu.Button;
                }
                else
                {
                    this.DrawLayout.HeadRightChildren.Add(menu.Button);
                }
            }
        }



        //Menu
        public void ConstructMenu(IMenu menu)
        {
            this.OverlayCanvas.Children.Add(menu.Layout);

            menu.Move += () =>
            {
                //Move to top
                int index = this.OverlayCanvas.Children.IndexOf(menu.Layout);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1);
            };
            menu.Opened += () => this.MenusDisable(menu);//Menus is disable
            menu.Closed += () => this.MenusEnable();//Menus is enable
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
                    menu.Layout.IsHitTestVisible = false;
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
                menu.Layout.IsHitTestVisible = true;
            }
            this.OverlayCanvas.Background = null;
        }


        #endregion
    }
}