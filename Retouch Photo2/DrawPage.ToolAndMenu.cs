using Retouch_Photo2.Menus;
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
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                this.ConstructMenuButton(menu);
                this.ConstructMenuLayout(menu);
            }

            this.OverlayCanvas.Tapped += (s, e) =>
            {
                foreach (IMenu menu in this.TipViewModel.Menus)
                {
                    menu.Expander.HideLayout();
                }
                this.IsOverlayDismiss = false;
            };

            this.OverlayCanvas.SizeChanged += (s, e) =>
            {
                foreach (IMenu menu in this.TipViewModel.Menus)
                {
                    menu.Expander.CropLayout();
                }
                this.IsOverlayDismiss = false;
            };
        }


        //Menu
        public void ConstructMenuButton(IMenu menu)
        {
            if (menu == null) return;
            UIElement button = menu.Expander.Button.Self;

            if (menu.Type == MenuType.Layer)
            {
                this.LayersControl.IndicatorBorder.Child = button;
            }
            else
            {
                this.MenuHead.Add(button);
            }
        }

        public void ConstructMenuLayout(IMenu menu)
        {
            if (menu == null) return;
            FrameworkElement layout = menu.Expander.Layout;

            
            //Move the menu to top.
            menu.Expander.Move += () =>
            {
                int index = this.OverlayCanvas.Children.IndexOf(layout);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1);
            };

            //Disable all menus, except the current menu.
            menu.Expander.Opened += () =>
            {
                foreach (IMenu m in this.TipViewModel.Menus)
                {
                     m.Expander.Layout.IsHitTestVisible = false;
                }
                menu.Expander.Layout.IsHitTestVisible = true;

                this.OverlayCanvas.Children.Add(layout);
                this.IsOverlayDismiss = true;
            };

            //Enable all menus.
            menu.Expander.Closed += () =>
            {
                foreach (IMenu m in this.TipViewModel.Menus)
                {
                    m.Expander.Layout.IsHitTestVisible = true;
                }

                this.OverlayCanvas.Children.Remove(layout);
                this.IsOverlayDismiss = false;
            };

            //Enable all menus.
            menu.Expander.Overlaid += () =>
            {
                foreach (IMenu m in this.TipViewModel.Menus)
                {
                    m.Expander.Layout.IsHitTestVisible = true;
                }

                this.IsOverlayDismiss = false;
            };
        }

    }
}