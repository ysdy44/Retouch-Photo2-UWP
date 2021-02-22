// Core:              ★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a canvas control, that containing some <see cref="Expander"/>。
    /// </summary>
    public class MenusExpanderCanvas : Canvas
    {

        /// <summary>
        /// Gets or sets the menus.
        /// </summary>
        public IList<IMenu> Menus
        {
            private get => this.menus;
            set
            {
                foreach (IMenu menu in value)
                {
                    if (menu == null) continue;

                    Expander expander = menu.Self;
                    expander.OverlayCanvas = this;
                    this.Children.Add(expander);
                }

                this.menus = value;
            }
        }
        private IList<IMenu> menus = null;

        internal bool IsOverlayDismiss
        {
            set
            {
                if (value)
                    this.Background = new SolidColorBrush(Colors.Transparent);
                else
                    this.Background = null;
            }
        }

        //@Construct
        /// <summary>
        /// Initializes a MenusExpanderCanvas. 
        /// </summary>
        public MenusExpanderCanvas()
        {
            this.Tapped += (s, e) =>
            {
                foreach (IMenu menu in this.Menus)
                {
                    if (menu == null) continue;

                    menu.HideLayout();
                }
                this.IsOverlayDismiss = false;
            };

            this.SizeChanged += (s, e) =>
            {
                foreach (IMenu menu in this.Menus)
                {
                    if (menu == null) continue;

                    menu.CropLayout();
                }
                this.IsOverlayDismiss = false;
            };
        }

    }
}