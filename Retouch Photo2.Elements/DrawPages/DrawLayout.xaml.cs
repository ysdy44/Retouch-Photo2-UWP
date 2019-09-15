using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.DrawPages
{
    /// <summary> 
    /// <see cref = "DrawPage" />'s layout. 
    /// </summary>
    public sealed partial class DrawLayout : UserControl
    {
        //@Content
        /// <summary> BackButton. </summary>
        public Button BackButton => this._BackButton;
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        /// <summary> IconLeftIcon's Content. </summary>
        public object ShowIcon { get => this.IconLeftContentControl.Content; set => this.IconLeftContentControl.Content = value; }
        /// <summary> IconRightIcon's Content. </summary>
        public object Icon { get => this.IconRightContentControl.Content; set => this.IconRightContentControl.Content = value; }

        /// <summary> TouchbarBorder's Child. </summary>
        public UIElement Touchbar { get => this.TouchbarBorder.Child; set => this.TouchbarBorder.Child = value; }
        /// <summary> Gets or sets FootScrollViewer's content. </summary>
        public Page FootPage
        {
            get => this.footPage;
            set
            {
                //If you choose a different tool, PhoneState will hided.
                Page oldPage = this.footPage;

                if (value != oldPage)
                {
                    if (this.Manager.PhoneState != DrawLayoutStateManager.DrawLayoutPhoneState.Hided)
                    {
                        this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.Hided;
                        this.State = this.Manager.GetState();//State
                    }
                }

                this.FootScrollViewer.Content = value;
                this.footPage=value;
            }
        }
        private Page footPage;


        #region HeadLeft & HeadRight


        /// <summary> HeadLeftBorder's Child. </summary>
        public UIElement HeadLeftPane { get => this.HeadLeftBorder.Child; set => this.HeadLeftBorder.Child = value; }


        /// <summary> HeadRightStackPane. </summary>
        public StackPanel HeadRightStackPane { get; set; } = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        /// <summary> HeadRight's Expand. </summary>
        public bool IsHeadRightExpand
        {
            set
            {
                if (value)
                {
                    this.HeadRightScrollViewer.Content = null;
                    this.HeadRightExpandBorder.Child = this.HeadRightStackPane;
                }
                else
                {
                    this.HeadRightExpandBorder.Child = null;
                    this.HeadRightScrollViewer.Content = this.HeadRightStackPane;
                }
            }
        }


        #endregion


        #region DependencyProperty


        /// <summary> Backgroud's Color. </summary>
        public ElementTheme Theme
        {
            set
            {
                switch (value)
                {
                    case ElementTheme.Light:
                        this.LightStoryboard.Begin();//Storyboard
                        break;
                    case ElementTheme.Dark:
                        this.DarkStoryboard.Begin();//Storyboard
                        break;
                }
            }
        }


        /// <summary> Sets or Gets the page layout is full screen. </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "DrawLayout.IsFullScreen" /> dependency property. </summary>
        public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof(IsFullScreen), typeof(bool), typeof(DrawLayout), new PropertyMetadata(false, (sender, e) =>
        {
            DrawLayout con = (DrawLayout)sender;

            if (e.NewValue is bool value)
            {
                con.Manager.IsFullScreen = value;
                con.State = con.Manager.GetState();//State
            }
        }));
        
        
        #endregion


        /// <summary> Manager of <see cref="DrawLayout"/>. </summary>
        DrawLayoutStateManager Manager = new DrawLayoutStateManager();
        /// <summary> State of <see cref="DrawLayout"/>. </summary>
        public DrawLayoutState State
        {
            set
            {
                switch (value)
                {
                    case DrawLayoutState.None:
                        VisualStateManager.GoToState(this, this.Normal.Name, false);
                        break;

                    case DrawLayoutState.FullScreen:
                        VisualStateManager.GoToState(this, this.FullScreen.Name, false);
                        break;

                    case DrawLayoutState.Phone: 
                            this.IsHeadRightExpand = false;
                        VisualStateManager.GoToState(this, this.Phone.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowLeft:
                        this.IsHeadRightExpand = false;
                        VisualStateManager.GoToState(this, this.PhoneShowLeft.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowRight:
                        this.IsHeadRightExpand = false;
                        VisualStateManager.GoToState(this, this.PhoneShowRight.Name, false);
                        break;

                    case DrawLayoutState.Pad:
                        this.IsHeadRightExpand = true;
                        VisualStateManager.GoToState(this, this.Pad.Name, false);
                        break;

                    case DrawLayoutState.PC:
                        this.IsHeadRightExpand = true;
                        VisualStateManager.GoToState(this, this.PC.Name, false);
                        break;
                }
            }
        }
        
        //@Construct
        public DrawLayout()
        {
            this.InitializeComponent();

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.Manager.Width = e.NewSize.Width;
                this.State = this.Manager.GetState(); //State
            };

            //HeadRight
            this.HeadRightToggleButton.Checked += (s, e) => this.HeadRightScrollViewer.Visibility = Visibility.Visible;
            this.HeadRightToggleButton.Unchecked += (s, e) => this.HeadRightScrollViewer.Visibility = Visibility.Collapsed;

            //DismissOverlay
            this.IconDismissOverlay.Tapped += (s, e) =>
            {
                this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.Hided;
                this.State = this.Manager.GetState();//State
            };

            //IconLeft
            this.IconLeftGrid.Tapped += (s, e) =>
            {
                this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.ShowLeft;
                this.State = this.Manager.GetState();//State
            };
            this.IconLeftGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.ShowLeft;
                    this.State = this.Manager.GetState();//State
                }
            };
            //IconRight
            this.IconRightGrid.Tapped += (s, e) =>
            {
                this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.ShowRight;
                this.State = this.Manager.GetState();//State
            };
            this.IconRightGrid.PointerEntered += (s, e) =>
            {
                if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.ShowRight;
                    this.State = this.Manager.GetState();//State
                }
            };
        }
    }     
}