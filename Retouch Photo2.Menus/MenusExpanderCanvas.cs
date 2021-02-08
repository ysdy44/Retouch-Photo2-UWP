// Core:              ★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★
using Retouch_Photo2.Elements;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a canvas control, that containing some <see cref="Expander"/>。
    /// </summary>
    public class MenusExpanderCanvas : UserControl
    {

        /// <summary>
        /// Gets or sets the menus/
        /// </summary>
        public IList<IMenu> Menus
        {
            private get => this.menus;
            set
            {
                foreach (IMenu menu in value)
                {
                    if (menu == null) continue;

                    FrameworkElement layout = menu.Self;
                    Expander.OverlayCanvas.Children.Add(layout);
                }
                this.menus = value;
            }
        }
        private IList<IMenu> menus = null;


        //@Construct
        /// <summary>
        /// Initializes a MenusExpanderCanvas. 
        /// </summary>
        public MenusExpanderCanvas()
        {
            this.Content = Expander.OverlayCanvas;

            Expander.OverlayCanvas.Tapped += (s, e) =>
            {
                foreach (IMenu menu in this.Menus)
                {
                    if (menu == null) continue;

                    menu.HideLayout();
                }
                Expander.IsOverlayDismiss = false;
            };

            Expander.OverlayCanvas.SizeChanged += (s, e) =>
            {
                foreach (IMenu menu in this.Menus)
                {
                    if (menu == null) continue;

                    menu.CropLayout();
                }
                Expander.IsOverlayDismiss = false;
            };
        }

    }
}