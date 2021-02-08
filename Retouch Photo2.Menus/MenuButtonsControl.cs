// Core:              ★
// Referenced:   ★
// Difficult:         
// Only:              ★★★
// Complete:      
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a stack panel control that containing some <see cref="MenuButton"/>。
    /// </summary>
    public sealed partial class MenuButtonsControl : UserControl
    {

        /// <summary>
        /// Gets or sets the menus.
        /// </summary>
        public IList<IMenu> Menus
        {
            set
            {
                foreach (IMenu menu in value)
                {
                    if (menu == null || menu.Button == null) continue;

                    FrameworkElement element = menu.Button.Self;
                    this.StackPanel.Children.Add(element);
                }
            }
        }

        private readonly StackPanel StackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };

        //@Construct
        /// <summary>
        /// Initializes a MenuButtonsControl. 
        /// </summary>
        public MenuButtonsControl()
        {
            this.Content = this.StackPanel;
        }

    }
}