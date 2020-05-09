using Retouch_Photo2.Elements;
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

        /// <summary>
        /// True if lightweight elimination is enabled for this control;
        /// </summary>
        bool IsOverlayDismiss
        {
            set
            {
                if (value)
                    this.OverlayCanvas.Background = new SolidColorBrush(Colors.Transparent);
                else
                    this.OverlayCanvas.Background = null;
            }
        }

        /// <summary> Head panel of Menu. </summary>
        UIElementCollection MenuHead => this.DrawLayout.HeadRightChildren;

        //Menu
        private void ConstructMenus()
        {
            //Menu
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenu(menu);
            }
            this.OverlayCanvas.Tapped += (s, e) => this.MenusHide();
            this.OverlayCanvas.SizeChanged += (s, e) => this.MenusHideAndCrop();
        }
               
        //Menu
        public void ConstructMenu(IMenu menu)
        {
            if (menu == null) return;

            this.OverlayCanvas.Children.Add(menu.Self);

            menu.Expander.Move += () =>
            {
                //Move to top
                int index = this.OverlayCanvas.Children.IndexOf(menu.Self);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1);
            };
            menu.Expander.Show += () => this.IsOverlayDismiss = true;
            menu.Expander.Opened += () => this.MenusDisable(menu);//Menus is disable
            menu.Expander.Closed += () => this.MenusEnable();//Menus is enable

            //MenuButton
            if (menu.Type == MenuType.Layer)
            {
                this.LayersControl.IndicatorBorder.Child = menu.Button.Self;
            }
            else
            {
                this.MenuHead.Add(menu.Button.Self);
            }
        }


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
                switch (menu.Expander.State)
                {
                    case ExpanderState.FlyoutShow:
                        menu.State = ExpanderState.Hide;
                        break;
                    case ExpanderState.Overlay:
                    case ExpanderState.OverlayNotExpanded:
                        if (isCrop)
                        {
                            Point postion = VisualUIElementHelper.GetOverlayPostion(menu.Self);
                            Point postion2 = VisualUIElementHelper.GetBoundPostion(postion, menu.Self);
                            VisualUIElementHelper.SetOverlayPostion(menu.Self, postion2);
                        }
                        break;
                }
            }

            this.IsOverlayDismiss = false;
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

            this.IsOverlayDismiss = true;
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


    }
}