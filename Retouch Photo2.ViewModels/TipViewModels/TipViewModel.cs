// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents an ViewModel that contains <see cref="ITool"/> <see cref="IMenu"/> and <see cref="ToolTip.IsOpen"/>
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the <see cref="ToolTip.IsOpen"/>. </summary>
        public bool IsOpen
        {
            get => this.isOpen;
            set
            {
                this.isOpen = value;
                this.OnPropertyChanged(nameof(this.IsOpen));//Notify 

                this.MenusIsOpen = value;
                this.ToolsIsOpen = value;
            }
        }
        private bool isOpen;


        #region Menu


        /// <summary> Gets or sets the all menus. </summary>   
        public IList<IMenu> Menus { get; set; } = new List<IMenu>();

        private bool MenusIsOpen
        {
            set
            {
                foreach (IMenu menu in this.Menus)
                {
                    if (menu == null) continue;
                    if (menu.Button == null) continue;
                    if (menu.Button.Self.Visibility == Visibility.Collapsed) continue;

                    //MenuButton
                    menu.Button.IsOpen = value;

                    //Menu
                    if (value)
                    {
                        if (menu.IsSecondPage) continue;
                        if (menu.State != ExpanderState.Overlay) continue;
                        menu.IsOpen = true;
                    }
                    else menu.IsOpen = false;
                }
            }
        }


        /// <summary>
        /// Show the layout.
        /// </summary>
        /// <param name="type"> The menu type. </param>
        public void ShowMenuLayout(MenuType type = MenuType.Layer)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {
                    switch (menu.State)
                    {
                        case ExpanderState.Hide:
                            menu.State = ExpanderState.FlyoutShow;
                            break;
                        case ExpanderState.OverlayNotExpanded:
                            menu.State = ExpanderState.Overlay;
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// Show the layout relative to the placement target.
        /// </summary>
        /// <param name="type"> The menu type. </param>
        /// <param name="placementTarget"> The  placement target.</param>
        /// <param name="placementMode"> The  placement mode.</param>
        public void ShowMenuLayoutAt(MenuType type, FrameworkElement placementTarget, FlyoutPlacementMode placementMode = FlyoutPlacementMode.Top)
        {
            foreach (IMenu menu in this.Menus)
            {
                if (menu.Type == type)
                {

                    if (menu.State == ExpanderState.Hide)
                    {
                        menu.CalculatePostion(placementTarget, placementMode);

                        menu.State = ExpanderState.FlyoutShow;
                    }

                }
            }
        }


        #endregion


        #region Tool


        /// <summary> Gets or sets the all tools. </summary>   
        public IList<ITool> Tools { get; set; } = new List<ITool>();

        private bool ToolsIsOpen
        {
            set
            {
                foreach (ITool tool in this.Tools)
                {
                    if (tool == null) continue;

                    tool.IsOpen = value;
                }
            }
        }


        #endregion


        //@Notify 
        /// <summary> Multicast event for property change notifications. </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName"> Name of the property used to notify listeners. </param>
        protected void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}