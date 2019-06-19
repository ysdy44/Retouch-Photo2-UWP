using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// MenuLayout:
    /// A layout control with Flyout and Overlay Layout.
    /// It can be moved on the screen.
    /// </summary>
    public sealed partial class MenuLayout : UserControl
    {

        //@Content
        /// <summary> Flyout's Placement. </summary>
        public FlyoutPlacementMode Placement { set => this.Flyout.Placement = value; get => this.Flyout.Placement; }

        /// <summary> TextBlock's Text. </summary>
        public string Text { set => this.MenuLayoutContent.TextBlock.Text = value; get => this.MenuLayoutContent.TextBlock.Text; }
        /// <summary> Title's Icon. </summary>
        public UIElement Icon { set => this.MenuLayoutContent.IconViewBox.Child = value; get => this.MenuLayoutContent.IconViewBox.Child; }

        /// <summary> TitleContentBorders Child. </summary>
        public UIElement ContentChild { get; set; }
        /// <summary> Flyout or Root. </summary>
        public MenuLayoutContent MenuLayoutContent { get; private set; } = new MenuLayoutContent();


        #region DependencyProperty


        /// <summary> The element to be used as the target for the location of the flyout. </summary>
        public FrameworkElement PlacementTarget
        {
            get { return (FrameworkElement)GetValue(PlacementTargetProperty); }
            set { SetValue(PlacementTargetProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MenuLayout.PlacementTarget" /> dependency property. </summary>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register(nameof(PlacementTarget), typeof(FrameworkElement), typeof(MenuLayout), new PropertyMetadata(null));


        /// <summary> State of <see cref="MenuLayout"/>. </summary>
        public MenuLayoutState State
        {
            get { return (MenuLayoutState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MenuLayout.State" /> dependency property. </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(MenuLayoutState), typeof(MenuLayout), new PropertyMetadata(MenuLayoutState.FlyoutHide, (sender, e) =>
        {
            MenuLayout con = (MenuLayout)sender;

            if (e.NewValue is MenuLayoutState value)
            {
                switch (value)
                {
                    case MenuLayoutState.FlyoutHide:
                        {
                            con.FlyoutOrRoot = true;
                            con.HideOrShow = true;

                            //Flyout
                            con.Flyout.Hide();
                        }
                        break;
                    case MenuLayoutState.FlyoutShow:
                        {
                            con.FlyoutOrRoot = true;
                            con.HideOrShow = false;

                            //Flyout
                            if (con.PlacementTarget != null) con.Flyout.ShowAt(con.PlacementTarget);
                        }
                        break;
                    case MenuLayoutState.RootExpanded:
                        {
                            con.FlyoutOrRoot = false;
                            con.HideOrShow = false;

                            //Flyout
                            con.Flyout.Hide();
                        }
                        break;
                    case MenuLayoutState.RootNotExpanded:
                        {
                            con.FlyoutOrRoot = false;
                            con.HideOrShow = true;

                            //Flyout
                            con.Flyout.Hide();
                        }
                        break;
                }
            }
        }));
        

        #endregion


        bool FlyoutOrRoot
        {
            set
            {
                if (value)
                {
                    //Flyout or Root
                    this.RootBorder.Child = null;
                    this.FlyoutBorder.Child = this.MenuLayoutContent;

                    //Background
                    this.Visibility = Visibility.Collapsed;
                    this.MenuLayoutContent.StoryboardRectangle.Visibility = Visibility.Collapsed;

                    //Close
                    this.MenuLayoutContent.CloseButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    //Flyout or Root
                    this.FlyoutBorder.Child = null;
                    this.RootBorder.Child = this.MenuLayoutContent;

                    //Background
                    this.Visibility = Visibility.Visible;
                    this.MenuLayoutContent.StoryboardRectangle.Visibility = Visibility.Visible;

                    //Close
                    this.MenuLayoutContent.CloseButton.Visibility = Visibility.Visible;
                }
            }
        }        

        bool HideOrShow
        {
            set
            {
                if (value)
                {
                    //Icon
                    this.MenuLayoutContent.StateIcon.Glyph = "\uE196";

                    //ContentBorder
                    this.MenuLayoutContent.ContentBorder.Child = null; 
                }
                else
                {
                    //Icon
                    this.MenuLayoutContent.StateIcon.Glyph = "\uE141";

                    //ContentBorder
                    this.MenuLayoutContent.ContentBorder.Child = this.ContentChild;
                }
            }
        }


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

        
        //@Construct
        public MenuLayout()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) => this.ControlSize = e.NewSize;

            //State
            this.State = MenuLayoutState.FlyoutHide;
            this.Flyout.Closed += (s, e) =>
            {
                if (this.State== MenuLayoutState.FlyoutShow)
                {
                    this.State = MenuLayoutState.FlyoutHide;
                }
            };

            //Button
            this.MenuLayoutContent.CloseButton.Tapped += (s, e) =>  this.State = MenuLayoutState.FlyoutHide;              
            this.MenuLayoutContent.StateButton.Tapped += (s, e) =>
            {
                if (this.State == MenuLayoutState.RootExpanded) this.State = MenuLayoutState.RootNotExpanded;
                else if (this.State == MenuLayoutState.RootNotExpanded) this.State = MenuLayoutState.RootExpanded;
                else
                {
                    Vector2 postion = this.GetElementVisualPostion(this.MenuLayoutContent.TitlePanel);
                    this.SetElementCanvasPostion(this, postion, this.ControlSize);

                    this.State = MenuLayoutState.RootExpanded;
                }
            };

            //Postion 
            this.MenuLayoutContent.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.MenuLayoutContent.TitlePanel.ManipulationStarted += (s, e) =>this.Postion = this.GetElementVisualPostion(this);
            this.MenuLayoutContent.TitlePanel.ManipulationDelta += (s, e) =>
            {
                if (this.State == MenuLayoutState.FlyoutShow) return;
                this.Postion += e.Delta.Translation.ToVector2();
                this.SetElementCanvasPostion(this, this.Postion, this.ControlSize);
            };
        }
    }
}