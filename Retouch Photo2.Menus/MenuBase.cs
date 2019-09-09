using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu base class that provides methods and properties for IMenu.
    /// </summary>
    public abstract class MenuBase
    {
        //@Content
        public abstract IMenuLayout MenuLayout { get; }
        public abstract IMenuButton MenuButton { get; }

        public UIElement MenuOverlay => this.menuOverlay; 
        private readonly Border menuOverlay = new Border();

        readonly MenuSize _menuSize = new MenuSize();


        #region State


        /// <summary> State of MenuBase. </summary>
        public MenuState State
        {
            get => this.state;
            set
            {
                if (this.MenuButton != null) this.MenuButton.State = value;
                if (this.MenuLayout != null) this.MenuLayout.State = value;

                switch (value)
                {
                    case MenuState.FlyoutHide:
                        {
                            this.FlyoutOrRoot = true;

                            this.MenuLayout.Flyout.Hide();
                        }
                        break;
                    case MenuState.FlyoutShow:
                        {
                            this.FlyoutOrRoot = true;

                            if (this.MenuButton != null)
                            {
                                this.MenuLayout.Flyout.ShowAt(this.MenuButton.Self);
                            }
                        }
                        break;
                    case MenuState.RootExpanded:
                        {
                            this.FlyoutOrRoot = false;

                            this.MenuLayout.Flyout.Hide();
                        }
                        break;
                    case MenuState.RootNotExpanded:
                        {
                            this.FlyoutOrRoot = false;

                            this.MenuLayout.Flyout.Hide();
                        }
                        break;
                }


                this.state = value;
            }
        }
        private MenuState state;


        /// <summary> Flyout or Root </summary>
        private bool FlyoutOrRoot
        {
            set
            {
                if (value)
                {
                    //Flyout or Root
                    this.menuOverlay.Child = null;
                    this.MenuLayout.Flyout.Content = this.MenuLayout.Self;
                    this.MenuOverlay.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Flyout or Root
                    this.MenuLayout.Flyout.Content = null;
                    this.menuOverlay.Child = this.MenuLayout.Self;
                    this.MenuOverlay.Visibility = Visibility.Visible;
                }
            }
        }


        #endregion
        

        //@Construct  
        public MenuBase()
        {
            if (this.MenuButton.Type != MenuButtonType.LayersControl)
            {
                this.MenuButton.Self.Tapped += (s, e) =>
                {
                    MenuState state = MenuBase.GetState(this.State);
                    this.State = state;
                };
            }
            
            this.State = MenuState.FlyoutHide;

            //Layout
            this.menuOverlay.SizeChanged += (s, e) => this._menuSize.Size = e.NewSize;

            //State
            this.MenuLayout.Flyout.Closed += (s, e) =>
            {
                if (this.State == MenuState.FlyoutShow)
                {
                    this.State = MenuState.FlyoutHide;
                }
            };

            //Button
            this.MenuLayout.CloseButton.Tapped += (s, e) => this.State = MenuState.FlyoutHide;
            this.MenuLayout.StateButton.Tapped += (s, e) =>
            {
                if (this.State == MenuState.RootExpanded) this.State = MenuState.RootNotExpanded;
                else if (this.State == MenuState.RootNotExpanded) this.State = MenuState.RootExpanded;
                else
                {
                    Vector2 postion = MenuSize.GetElementVisualPostion(this.MenuLayout.TitlePanel);
                    MenuSize.SetElementCanvasPostion(this.MenuOverlay, postion, this._menuSize.Size);

                    this.State = MenuState.RootExpanded;
                }
            };

            //Postion 
            this.MenuLayout.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.MenuLayout.TitlePanel.ManipulationStarted += (s, e) => this._menuSize.Postion = MenuSize.GetElementVisualPostion(this.MenuOverlay);
            this.MenuLayout.TitlePanel.ManipulationDelta += (s, e) =>
            {
                if (this.State == MenuState.FlyoutShow) return;
                this._menuSize.Postion += e.Delta.Translation.ToVector2();
                MenuSize.SetElementCanvasPostion(this.MenuOverlay, this._menuSize.Postion, this._menuSize.Size);
            };
        }


        //@Static
        /// <summary>
        /// Get the corresponding status. 
        /// </summary>
        /// <param name="state"> The source state. </param>
        /// <returns> The destination state. </returns>
        private static MenuState GetState(MenuState state)
        {
            switch (state)
            {
                case MenuState.FlyoutHide:
                    return MenuState.FlyoutShow;
                case MenuState.FlyoutShow:
                    return MenuState.FlyoutHide;

                case MenuState.RootExpanded:
                    return MenuState.RootNotExpanded;
                case MenuState.RootNotExpanded:
                    return MenuState.RootExpanded;
            }
            return MenuState.FlyoutShow;
        }
    }
}
