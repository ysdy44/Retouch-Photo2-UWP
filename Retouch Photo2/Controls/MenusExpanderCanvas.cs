using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Controls
{
    /// <summary>
    /// Represents a canvas control, that containing some <see cref="Expander"/>。
    /// </summary>
    public class MenusExpanderCanvas : UserControl
    {
        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        readonly Canvas OverlayCanvas = new Canvas();


        /// <summary>
        /// True if lightweight elimination is enabled for this control;
        /// </summary>
        public bool IsOverlayDismiss
        {
            set
            {
                if (value)
                    this.OverlayCanvas.Background = new SolidColorBrush(Colors.Transparent);
                else
                    this.OverlayCanvas.Background = null;
            }
        }

        //@Construct
        /// <summary>
        /// Initializes a MenusExpanderCanvas. 
        /// </summary>
        public MenusExpanderCanvas()
        {
            this.Content = this.OverlayCanvas;
            this.ConstructMenus();
        }


        //Menu
        private void ConstructMenus()
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
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


        private void ConstructMenuLayout(IMenu menu)
        {
            if (menu == null) return;
            FrameworkElement layout = menu.Expander.Layout;
            this.OverlayCanvas.Children.Add(layout);


            //Move the menu to top.
            menu.Expander.Move += () =>
            {
                int index = this.OverlayCanvas.Children.IndexOf(layout);
                int count = this.OverlayCanvas.Children.Count;
                this.OverlayCanvas.Children.Move((uint)index, (uint)count - 1); ;
            };

            //Disable all menus, except the current menu.
            menu.Expander.Opened += () =>
            {
                foreach (IMenu m in this.TipViewModel.Menus)
                {
                    m.Expander.Layout.IsHitTestVisible = false;
                }
                menu.Expander.Layout.IsHitTestVisible = true;

                menu.Expander.Move();
                layout.Visibility = Visibility.Visible;
                this.IsOverlayDismiss = true;
            };

            //Enable all menus.
            menu.Expander.Closed += () =>
            {
                foreach (IMenu m in this.TipViewModel.Menus)
                {
                    m.Expander.Layout.IsHitTestVisible = true;
                }

                layout.Visibility = Visibility.Collapsed;
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
