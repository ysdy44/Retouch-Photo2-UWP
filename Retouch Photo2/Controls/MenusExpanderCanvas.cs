// Core:              ★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary>
    /// Represents a canvas control, that containing some <see cref="Expander"/>。
    /// </summary>
    public class MenusExpanderCanvas : UserControl
    {

        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
               

        //@Construct
        /// <summary>
        /// Initializes a MenusExpanderCanvas. 
        /// </summary>
        public MenusExpanderCanvas()
        {
            this.Content = Expander.OverlayCanvas;
            this.ConstructMenus();
        }


        //Menu
        private void ConstructMenus()
        {
            foreach (IMenu menu in this.TipViewModel.Menus)
            {
                if (menu != null)
                {
                    FrameworkElement layout = menu.Self;
                    Expander.OverlayCanvas.Children.Add(layout);
                }
            }

            Expander.OverlayCanvas.Tapped += (s, e) =>
            {
                foreach (IMenu menu in this.TipViewModel.Menus)
                {
                    menu.HideLayout();
                }
                Expander.IsOverlayDismiss = false;
            };

            Expander.OverlayCanvas.SizeChanged += (s, e) =>
            {
                foreach (IMenu menu in this.TipViewModel.Menus)
                {
                    menu.CropLayout();
                }
                Expander.IsOverlayDismiss = false;
            };
        }         

    }
}