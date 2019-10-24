using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu base class that provides methods and properties for IMenu.
    /// </summary>
    public abstract class MenuBase
    {
        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }

        //@Content
        public abstract IMenuLayout Layout { get; }
        public abstract IMenuButton Button { get; }
        public bool IsHitTestVisible
        {
            set
            {
                if (value)
                {
                    this.Layout.Self.IsHitTestVisible = true;
                }
                else
                {
                    if (this.State == MenuState.FlyoutHide) return;
                    if (this.State == MenuState.FlyoutShow) return;

                    this.Layout.Self.IsHitTestVisible = false;
                }
            }
        }

        Point _postion;
        public static Action<string> dsfsdfsdfsd;
        /// <summary> State of MenuBase. </summary>
        public MenuState State
        {
            get => this.state;
            set
            {
                this.Button.State = value;
                this.Layout.State = value;

                this.Layout.Self.Visibility = (value == MenuState.FlyoutHide) ? Visibility.Collapsed : Visibility.Visible;

                if (value == MenuState.FlyoutShow)
                {
                    FlyoutPlacementMode placement = (this.Button.Type == MenuButtonType.None) ? FlyoutPlacementMode.Bottom : FlyoutPlacementMode.Left;
                    Point postion = MenuHelper.GetFlyoutPostion(this.Button.Self, this.Layout.Self, placement);
                    Point postion2 = MenuHelper.GetBoundPostion(postion, this.Layout.Self);
                    MenuHelper.SetOverlayPostion(this.Layout.Self, postion2);
                    this.Move?.Invoke(); //Delegate

                    if (this.state == MenuState.FlyoutHide) this.Opened?.Invoke(); //Delegate 
                }
                else
                {
                    if (this.state == MenuState.FlyoutShow) this.Closed?.Invoke(); //Delegate
                }

                this.state = value;
            }
        }
        private MenuState state;

        //@Construct  
        public MenuBase()
        {
            this.State = MenuState.FlyoutHide;

            if (this.Button.Type == MenuButtonType.None)
            {
                this.Button.Self.Tapped += (s, e) =>
                {
                    this.State = this.GetState(this.State);
                };
            }

            //Button
            this.Layout.CloseButton.Tapped += (s, e) => this.State = MenuState.FlyoutHide;
            this.Layout.StateButton.Tapped += (s, e) => this.State = this.GetState2(this.State);

            //Postion 
            this.Layout.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.Layout.TitlePanel.ManipulationStarted += (s, e) =>
            {
                this.Layout.Self.Opacity = 0.6d;
                this._postion = MenuHelper.GetVisualPostion(this.Layout.Self);
                this.Move?.Invoke(); //Delegate
            };
            this.Layout.TitlePanel.ManipulationDelta += (s, e) =>
            {
                if (this.State != MenuState.FlyoutShow)
                {
                    this._postion.X += e.Delta.Translation.X;
                    this._postion.Y += e.Delta.Translation.Y;
                    Point postion2 = MenuHelper.GetBoundPostion(this._postion, this.Layout.Self);
                    MenuHelper.SetOverlayPostion(this.Layout.Self, postion2);
                }
            };
            this.Layout.TitlePanel.ManipulationCompleted += (s, e) =>
            {
                this.Layout.Self.Opacity = 1.0d;
            };
        }

        private MenuState GetState(MenuState state)
        {
            switch (state)
            {
                case MenuState.FlyoutHide: return MenuState.FlyoutShow;
                case MenuState.FlyoutShow: return MenuState.FlyoutHide;

                case MenuState.OverlayExpanded: return MenuState.OverlayNotExpanded;
                case MenuState.OverlayNotExpanded: return MenuState.OverlayExpanded;
            }
            return MenuState.FlyoutShow;
        }
        private MenuState GetState2(MenuState state)
        {
            switch (state)
            {
                case MenuState.OverlayExpanded: return MenuState.OverlayNotExpanded;
                case MenuState.OverlayNotExpanded: return MenuState.OverlayExpanded;
            }
            return MenuState.OverlayExpanded;
        }
    }
}