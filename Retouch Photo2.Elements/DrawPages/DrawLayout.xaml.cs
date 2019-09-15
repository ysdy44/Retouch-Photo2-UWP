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
        /// <summary> CenterBorder's Child. </summary>
        public UIElement CenterChild { get => this.CenterBorder.Child; set => this.CenterBorder.Child = value; }
        /// <summary> RightBorder's Child. </summary>
        public UIElement RightPane { get => this.RightBorder.Child; set => this.RightBorder.Child = value; }
        /// <summary> LeftBorder's Child. </summary>
        public UIElement LeftPane { get => this.LeftBorder.Child; set => this.LeftBorder.Child = value; }
        
        /// <summary> BackButton. </summary>
        public Button BackButton { get => this._BackButton; set => this._BackButton = value; }
        /// <summary> HeadRightStackPanel's Children. </summary>
        public UIElementCollection HeadRightChildren => this.HeadRightStackPanel.Children;

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

                //Float
                this.FloatPartState = this.GetFloatState();
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


    public sealed partial class DrawLayout : UserControl
    {
        //@Content
        /// <summary> FloatTopStackPanelPart1's Children. </summary>
        public UIElementCollection FloatTopPart1Children => this.FloatTopStackPanelPart1.Children;
        /// <summary> FloatTopStackPanelPart2's Children. </summary>
        public UIElementCollection FloatTopPart2Children => this.FloatTopStackPanelPart2.Children;

        /// <summary> FloatBottomStackPanelPart1's Children. </summary>
        public UIElementCollection FloatBottomPart1Children => this.FloatBottomStackPanelPart1.Children;
        /// <summary> FloatBottomStackPanelPart2's Children. </summary>
        public UIElementCollection FloatBottomPart2Children => this.FloatBottomStackPanelPart2.Children;


        /// <summary> State of <see cref="DrawLayout"/>'s float part. </summary>
        public VerticalAlignment FloatPartState
        {
            set
            {
                switch (value)
                {
                    case VerticalAlignment.Top:
                        {
                            this.FloatTopStackPanel.Visibility = Visibility.Visible;
                            this.FloatTopStackPanelPart1.Visibility = Visibility.Visible;
                            this.FloatTopStackPanelPart2.Visibility = Visibility.Visible;

                            this.FloatBottomBorder.Visibility = Visibility.Collapsed;
                            this.FloatBottomStackPanelPart1.Visibility = Visibility.Collapsed;
                            this.FloatBottomStackPanelPart2.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case VerticalAlignment.Center:
                        {
                            this.FloatTopStackPanel.Visibility = Visibility.Visible;
                            this.FloatTopStackPanelPart1.Visibility = Visibility.Visible;
                            this.FloatTopStackPanelPart2.Visibility = Visibility.Collapsed;

                            this.FloatBottomBorder.Visibility = Visibility.Visible;
                            this.FloatBottomStackPanelPart1.Visibility = Visibility.Collapsed;
                            this.FloatBottomStackPanelPart2.Visibility = Visibility.Visible;
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        {
                            this.FloatTopStackPanel.Visibility = Visibility.Collapsed;
                            this.FloatTopStackPanelPart1.Visibility = Visibility.Collapsed;
                            this.FloatTopStackPanelPart2.Visibility = Visibility.Collapsed;

                            this.FloatBottomBorder.Visibility = Visibility.Visible;
                            this.FloatBottomStackPanelPart1.Visibility = Visibility.Visible;
                            this.FloatBottomStackPanelPart2.Visibility = Visibility.Visible;
                        }
                        break;
                }
            }
        }

        private VerticalAlignment GetFloatState()
        {
            double floatHeadWidth = this.FloatHeadColumnDefinition.ActualWidth - this.HeadRightScrollViewer.ExtentWidth; ;

            if (floatHeadWidth > 300)
                return VerticalAlignment.Top;
            else if (floatHeadWidth < 140)
                return VerticalAlignment.Bottom;
            else
                return VerticalAlignment.Center;
        }
    }
}