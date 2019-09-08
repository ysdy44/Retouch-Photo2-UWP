using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Menus
{
    public abstract class Menu
    {

        //@Content
        public abstract IMenuLayout Layout { get; }
        public abstract IMenuButton Button { get; }
        public Border Content { get; } = new Border();


        #region State


        /// <summary> State of <see cref="Menus.MenuLayout"/>. </summary>
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
                        {
                            this.FlyoutOrRoot = true;

                            this.Layout.Flyout.Hide();
                        }
                        break;
                    case MenuState.FlyoutShow:
                        {
                            this.FlyoutOrRoot = true;

                            if (this.Button != null)
                            {
                                if (this.Button.Self != null)
                                {
                                    this.Layout.Flyout.ShowAt(this.Button.Self);
                                }
                            }
                        }
                        break;
                    case MenuState.RootExpanded:
                        {
                            this.FlyoutOrRoot =false;

                            this.Layout.Flyout.Hide();
                        }
                        break;
                    case MenuState.RootNotExpanded:
                        {
                            this.FlyoutOrRoot = false;

                            this.Layout.Flyout.Hide();
                        }
                        break;
                }


                this.state = value;
            }
        }
        private MenuState state;


        /// <summary> FlyoutOrRoot </summary>
        private bool FlyoutOrRoot
        {
            set
            {
                if (value)
                {
                    //Flyout or Root
                    this.Content.Child = null;
                    this.Layout.Flyout.Content = this.Layout.Self;
                    this.Content.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Flyout or Root
                    this.Layout.Flyout.Content = null;
                    this.Content.Child = this.Layout.Self;
                    this.Content.Visibility = Visibility.Visible;
                }
            }
        }


        #endregion


        #region Postion


        //Postion: the position of the Root on the canvas.
        private Size ControlSize;
        private Vector2 Postion;
        /// <summary> Gets Flyout's postion. </summary>
        private Vector2 GetElementVisualPostion(UIElement element) => element.TransformToVisual(Window.Current.Content).TransformPoint(new Point()).ToVector2();
        /// <summary> Gets Root's postion. </summary>
        private Vector2 GetElementCanvasPostion(UIElement element) => new Vector2((float)Canvas.GetLeft(element), (float)Canvas.GetTop(element));
        /// <summary> Sets Root's postion. </summary>
        private void SetElementCanvasPostion(UIElement element, Vector2 postion, Size size)
        {
            double X;
            if (postion.X < 0) X = 0;
            else if (size.Width > Window.Current.Bounds.Width) X = 0;
            else if (postion.X > (Window.Current.Bounds.Width - size.Width)) X = (Window.Current.Bounds.Width - size.Width);
            else X = postion.X;
            Canvas.SetLeft(element, X);

            double Y;
            if (postion.Y < 0) Y = 0;
            else if (size.Height > Window.Current.Bounds.Height) Y = 0;
            else if (postion.Y > (Window.Current.Bounds.Height - size.Height)) Y = (Window.Current.Bounds.Height - size.Height);
            else Y = postion.Y;
            Canvas.SetTop(element, Y);
        }


        #endregion


        //@Construct  
        public Menu()
        {
            this.Button.Self.Tapped += (s, e) =>
            {
                MenuState state = Menu.GetState(this.State);
                this.State = state;
            };

            this.State = MenuState.FlyoutHide;

            //Layout
            this.Content.SizeChanged += (s, e) => this.ControlSize = e.NewSize;

            //State
            this.Layout.Flyout.Closed += (s, e) =>
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
                if (this.State == MenuState.RootExpanded) this.State = MenuState.RootNotExpanded;
                else if (this.State == MenuState.RootNotExpanded) this.State = MenuState.RootExpanded;
                else
                {
                    Vector2 postion = this.GetElementVisualPostion(this.Layout.TitlePanel);
                    this.SetElementCanvasPostion(this.Content, postion, this.ControlSize);

                    this.State = MenuState.RootExpanded;
                }
            };

            //Postion 
            this.Layout.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.Layout.TitlePanel.ManipulationStarted += (s, e) => this.Postion = this.GetElementVisualPostion(this.Content);
            this.Layout.TitlePanel.ManipulationDelta += (s, e) =>
            {
                if (this.State == MenuState.FlyoutShow) return;
                this.Postion += e.Delta.Translation.ToVector2();
                this.SetElementCanvasPostion(this.Content, this.Postion, this.ControlSize);
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
