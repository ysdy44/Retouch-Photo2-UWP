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
        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }

        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterContent { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }

        /// <summary> TopLeftBorder's Child. </summary>
        public UIElement TopLeftPane { get => this.TopLeftBorder.Child; set => this.TopLeftBorder.Child = value; }
        /// <summary> TopRightStackPanel's Children. </summary>
        public UIElementCollection TopRightPanelChildren => this.TopRightStackPanel.Children;

        /// <summary> TopLeftStackPanel's Children. </summary>
        public UIElementCollection TopLeftPanelChildren => this.TopLeftStackPanel.Children;


        //@Content  
        /// <summary> TouchbarBorder's Child. </summary>
        public UIElement Touchbar { get => this.TouchbarBorder.Child; set => this.TouchbarBorder.Child = value; }

        /// <summary> Gets or sets <see cref = "DrawLayout" />'s ShowIcon. </summary>
        public object ShowIcon { get => this.IconLeftIcon.Content; set => this.IconLeftIcon.Content = value; }
        /// <summary> Gets or sets <see cref = "DrawLayout" />'s Icon. </summary>
        public object Icon { get => this.IconRightIcon.Content; set => this.IconRightIcon.Content = value; }
        
        /// <summary> Gets or sets <see cref = "DrawLayout" />'s Page. </summary>
        public Page Page
        {
            get => this.page;
            set
            {
                //If you choose a different tool, PhoneState will hided.
                Page oldPage = this.page;

                if (value != oldPage)
                {
                    if (this.Manager.PhoneState != DrawLayoutStateManager.DrawLayoutPhoneState.Hided)
                    {
                        this.Manager.PhoneState = DrawLayoutStateManager.DrawLayoutPhoneState.Hided;
                        this.State = this.Manager.GetState();//State
                    }
                }

                this.ScrollViewer.Content = value;
                this.page=value;
            }
        }
        private Page page;


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
            get => this.state;
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
                        VisualStateManager.GoToState(this, this.Phone.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowLeft:
                        VisualStateManager.GoToState(this, this.PhoneShowLeft.Name, false);
                        break;
                    case DrawLayoutState.PhoneShowRight:
                        VisualStateManager.GoToState(this, this.PhoneShowRight.Name, false);
                        break;

                    case DrawLayoutState.Pad:
                        VisualStateManager.GoToState(this, this.Pad.Name, false);
                        break;

                    case DrawLayoutState.PC:
                        VisualStateManager.GoToState(this, this.PC.Name, false);
                        break;
                }
                this.state = value;
            }
        }
        private DrawLayoutState state;


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

            //FullScreen
            this.UnFullScreenButton.Tapped += (s, e) => this.IsFullScreen = !this.IsFullScreen;
            this.FullScreenButton.Tapped += (s, e) => this.IsFullScreen = !this.IsFullScreen;

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