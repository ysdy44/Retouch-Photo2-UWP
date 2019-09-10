using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu base class that provides methods and properties for IMenu.
    /// </summary>
    public abstract class MenuBase
    {
        //@Delegate
        public EventHandler<UIElement> Move { get; set; }
          
        //@Content
        public abstract IMenuLayout Layout { get; }
        public abstract IMenuButton Button { get; }

        public UIElement Overlay => this._overlay;
        private readonly Border _overlay = new Border();
        
        public FlyoutBase Flyout => this._flyout;
        private readonly Flyout _flyout = new Flyout
        {
            FlyoutPresenterStyle = new Style
            {
                TargetType = typeof(FlyoutPresenter),
                Setters =
                {
                    new Setter
                    {
                        Property = FlyoutPresenter.PaddingProperty,
                        Value = new Thickness(0,0,0,0),
                    },
                    new Setter
                    {
                        Property = FlyoutPresenter.MarginProperty,
                        Value = new Thickness(0,0,0,0),
                    },
                    new Setter
                    {
                        Property = FlyoutPresenter.BorderThicknessProperty,
                        Value = new Thickness(0,0,0,0),
                    },
                    new Setter
                    {
                        Property = FlyoutPresenter.BorderBrushProperty,
                        Value = new SolidColorBrush(Colors.Transparent),
                    },
                    new Setter
                    {
                        Property = FlyoutPresenter.BackgroundProperty,
                        Value = new SolidColorBrush(Colors.Transparent),
                    },
                }
            }
        };
        

        readonly MenuSize _size = new MenuSize();


        /// <summary> ToolTip IsOpen. </summary>
        public bool IsOpen
        {
            set
            {
                if (this.Layout == null) return;

                if (this.State == MenuState.OverlayExpanded)
                {
                    this.Layout.IsOpen = value;
                }
            }
        }
        /// <summary> State of MenuBase. </summary>
        public MenuState State
        {
            get => this.state;
            set
            {
                if (this.Button != null) this.Button.State = value;
                if (this.Layout != null) this.Layout.State = value;

                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.FlyoutShow:
                        {
                            this._overlay.Child = null;
                            this._flyout.Content = this.Layout.Self;
                            this._overlay.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case MenuState.OverlayExpanded:
                    case MenuState.OverlayNotExpanded:
                        {
                            this._flyout.Content = null;
                            this._overlay.Child = this.Layout.Self;
                            this._overlay.Visibility = Visibility.Visible;
                        }
                        break;
                }
                
                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.OverlayExpanded:
                    case MenuState.OverlayNotExpanded:
                        this._flyout.Hide();
                        break;
                    case MenuState.FlyoutShow:
                        if (this.Button == null) break;
                        this._flyout.ShowAt(this.Button.Self);
                        break;
                }

                this.state = value;
            }
        }
        private MenuState state;
         

        //@Construct  
        public MenuBase()
        {
            this.State = MenuState.FlyoutHide;

            //MenuButtonType
            switch (this.Button.Type)
            {
                case MenuButtonType.None:
                    {
                        this.Flyout.Placement = FlyoutPlacementMode.Bottom;
                        this.Button.Self.Tapped += (s, e) => this.State = MenuBase.GetState(this.State);
                    }
                    break;
                case MenuButtonType.ToolButton:
                    {
                        this.Flyout.Placement = FlyoutPlacementMode.Right;
                        this.Button.Self.Tapped += (s, e) => this.State = MenuBase.GetState(this.State);
                    }
                    break;
                case MenuButtonType.LayersControl:
                    {
                        this.Flyout.Placement = FlyoutPlacementMode.Left;
                    }
                    break;
            }
            
            //Layout
            this._overlay.SizeChanged += (s, e) => this._size.Size = e.NewSize;

            //State
            this._flyout.Closed += (s, e) =>
            {
                if (this.State == MenuState.FlyoutShow)
                {
                    this.State = MenuState.FlyoutHide;
                }
            };

            //Button
            this.Layout.CloseButton.Tapped += (s, e) => this.State = MenuState.FlyoutHide;
            this.Layout.StateButton.Tapped += (s, e) =>
            {
                if (this.State == MenuState.OverlayExpanded) this.State = MenuState.OverlayNotExpanded;
                else if (this.State == MenuState.OverlayNotExpanded) this.State = MenuState.OverlayExpanded;
                else
                {
                    Vector2 postion = MenuSize.GetElementVisualPostion(this.Layout.TitlePanel);
                    MenuSize.SetElementCanvasPostion(this.Overlay, postion, this._size.Size);

                    this.State = MenuState.OverlayExpanded;
                }
            };

            //Postion 
            this.Layout.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.Layout.TitlePanel.ManipulationStarted += (s, e) =>
            {
                this._size.Postion = MenuSize.GetElementVisualPostion(this.Overlay);
                this.Move?.Invoke(this, this.Overlay); //Delegate
            };
            this.Layout.TitlePanel.ManipulationDelta += (s, e) =>
            {
                if (this.State == MenuState.FlyoutShow) return;
                this._size.Postion += e.Delta.Translation.ToVector2();
                MenuSize.SetElementCanvasPostion(this.Overlay, this._size.Postion, this._size.Size);
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

                case MenuState.OverlayExpanded:
                    return MenuState.OverlayNotExpanded;
                case MenuState.OverlayNotExpanded:
                    return MenuState.OverlayExpanded;
            }
            return MenuState.FlyoutShow;
        }
    }
}
