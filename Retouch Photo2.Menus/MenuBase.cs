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
        public EventHandler<UIElement> Move { get; set; }
        public EventHandler<object> Closed { get; set; }
        public EventHandler<object> Opened { get; set; }

        //@Content
        public abstract IMenuLayout Layout { get; }
        public abstract IMenuButton Button { get; }

        Point _postion;

        /// <summary> State of MenuBase. </summary>
        public MenuState State
        {
            get => this.state;
            set
            {
                this.Button.State = value;
                this.Layout.State = value;

                if (value == MenuState.FlyoutShow)
                {
                    FlyoutPlacementMode placement = (this.Button.Type == MenuButtonType.None) ? FlyoutPlacementMode.Bottom : FlyoutPlacementMode.Left;
                    Point postion = MenuHelper.GetFlyoutPostion(this.Button.Self, this.Layout.Self, placement);
                    Point postion2 = MenuHelper.GetBoundPostion(postion, this.Button.Self);
                    MenuHelper.SetOverlayPostion(this.Layout.Self, postion2);
                    this.Move?.Invoke(this, this.Layout.Self); //Delegate

                    if (this.state == MenuState.FlyoutHide) this.Opened?.Invoke(this, null); //Delegate 
                }
                else
                {
                    if (this.state == MenuState.FlyoutShow) this.Closed?.Invoke(this, null); //Delegate
                }

                this.Layout.Self.Visibility = (value == MenuState.FlyoutHide) ? Visibility.Collapsed : Visibility.Visible;

                this.state = value;
            }
        }
        private MenuState state;

        //@Construct  
        public MenuBase()
        {
            this.State = MenuState.FlyoutHide;

            //Button
            this.Layout.CloseButton.Tapped += (s, e) => this.State = MenuState.FlyoutHide;
            this.Layout.StateButton.Tapped += (s, e) =>
            {
                if (this.State == MenuState.OverlayExpanded) this.State = MenuState.OverlayNotExpanded;
                else if (this.State == MenuState.OverlayNotExpanded) this.State = MenuState.OverlayExpanded;
                else
                {
                    Point postion = MenuHelper.GetVisualPostion(this.Layout.TitlePanel);
                    Point postion2 = MenuHelper.GetBoundPostion(postion, this.Button.Self);
                    MenuHelper.SetOverlayPostion(this.Layout.Self, postion2);

                    this.State = MenuState.OverlayExpanded;
                }
            };

            //Postion 
            this.Layout.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.Layout.TitlePanel.ManipulationStarted += (s, e) =>
            {
                this._postion = MenuHelper.GetVisualPostion(this.Layout.Self);
                this.Move?.Invoke(this, this.Layout.Self); //Delegate
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
        }
    }
}